﻿using System.Collections.Specialized;
using System.ComponentModel;
using DNTProfiler.ByException.Core;
using DNTProfiler.Common.Converters;
using DNTProfiler.Common.JsonToolkit;
using DNTProfiler.Common.Models;
using DNTProfiler.Common.Mvvm;
using DNTProfiler.Infrastructure.ViewModels;
using DNTProfiler.PluginsBase;

namespace DNTProfiler.ByException.ViewModels
{
    public class MainViewModel : MainViewModelBase
    {
        private readonly CallbacksManager _callbacksManager;

        public MainViewModel(ProfilerPluginBase pluginContext)
            : base(pluginContext)
        {
            if (Designer.IsInDesignModeStatic)
                return;

            setActions();
            setGuiModel();
            _callbacksManager = new CallbacksManager(PluginContext, GuiModelData);
            setEvenets();
        }

        private void GuiModelData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedApplicationIdentity":
                    _callbacksManager.ShowSelectedApplicationIdentityLocalCommands();
                    break;
                case "SelectedExecutedCommand":
                    _callbacksManager.ShowSelectedCommandRelatedStackTraces();
                    break;
            }
        }

        private void setActions()
        {
            PluginContext.Reset = () =>
            {
                ResetAll();
            };

            PluginContext.GetResults = () =>
            {
                return GuiModelData.RelatedCommands.ToFormattedJson();
            };
        }

        private void setEvenets()
        {
            PluginContext.ProfilerData.Results.CollectionChanged += Results_CollectionChanged; ;
        }

        void Results_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (CommandResult item in e.NewItems)
                    {
                        _callbacksManager.AddExceptionalCommands(item);
                    }
                    break;
            }
        }

        private void setGuiModel()
        {
            GuiModelData.PropertyChanged += GuiModelData_PropertyChanged;
        }
    }
}