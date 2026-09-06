namespace Tempest.UI.Infrastructure.Plasma;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tempest.UI.Infrastructure.Persistence;

internal class PlasmaGlobalConfigWatcher : IDisposable
{
    private readonly Lock _lock = new();
    
    private readonly string _targetFilePath;
    private readonly string _targetFileName;
    
    private readonly FileSystemWatcher _watcher;
    private readonly List<PlasmaConfigEntryCallbackContext> _callbackContexts = [];

    private KConfigFile? _kdeGlobalConfig;
    private bool _disposed;

    public PlasmaGlobalConfigWatcher(string targetFilePath)
    {
        if (!File.Exists(targetFilePath))
        {
            throw new FileNotFoundException(
                "Target configuration file does not exist.", 
                targetFilePath
            );
        }

        _targetFilePath = targetFilePath;
        _targetFileName = Path.GetFileName(targetFilePath);
        var targetDirectory = Path.GetDirectoryName(targetFilePath)!;

        _kdeGlobalConfig = new KConfigFile(_targetFilePath);
        _watcher = new FileSystemWatcher(targetDirectory)
        {
            Filter = _targetFileName,
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size,
            EnableRaisingEvents = true
        };

        _watcher.Changed += OnConfigFileActivity;
        _watcher.Created += OnConfigFileActivity;
        _watcher.Deleted += OnConfigFileDeleted;
    }

    public void WatchValue(string section, string key, Action<string> callback)
    {
        lock (_lock)
        {
            if (_callbackContexts.Any(c => c.Section.Equals(section, StringComparison.OrdinalIgnoreCase) 
                                        && c.Key.Equals(key, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"'{section}.{key}' is already being watched.");
            }

            var context = new PlasmaConfigEntryCallbackContext(section, key, callback);
            _callbackContexts.Add(context);

            var value = _kdeGlobalConfig?.GetValue(section, key);
            if (value != null)
            {
                callback(value);
            }
        }
    }

    public void UnwatchValue(string section, string key)
    {
        lock (_lock)
        {
            _callbackContexts.RemoveAll(
                c => c.Section.Equals(section, StringComparison.OrdinalIgnoreCase) 
                  && c.Key.Equals(key, StringComparison.OrdinalIgnoreCase)
            );
        }
    }

    private void OnConfigFileActivity(object sender, FileSystemEventArgs e)
    {
        // Plasma can flush its writes in rapid bursts or replace files atomically.
        // Add a brief delay to avoid any collisions with outside environment.
        Task.Delay(50).ContinueWith(_ =>
        {
            List<(Action<string> Callback, string Value)> notifications = [];

            lock (_lock)
            {
                if (!File.Exists(_targetFilePath))
                {
                    return;
                }

                _kdeGlobalConfig = new KConfigFile(_targetFilePath);

                foreach (var ctx in _callbackContexts)
                {
                    var val = _kdeGlobalConfig.GetValue(
                        ctx.Section, 
                        ctx.Key
                    );
                    
                    if (val != null)
                    {
                        notifications.Add((ctx.Callback, val));
                    }
                }
            }

            foreach (var (callback, value) in notifications)
            {
                callback(value);
            }
        });
    }

    private void OnConfigFileDeleted(object sender, FileSystemEventArgs e)
    {
        lock (_lock)
        {
            _kdeGlobalConfig = null;
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        
        _watcher.Dispose();
        _disposed = true;
    }
}