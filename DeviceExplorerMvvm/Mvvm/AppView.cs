using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Shell;

namespace DeviceExplorer.Mvvm
{
    public class AppView : Window
    {
        public AppView()
        {
            this.ResizeMode = ResizeMode.CanResizeWithGrip;
            this.SetBinding(Window.TitleProperty, new Binding("Title"));

            TaskbarItemInfo taskbarItemInfo = new TaskbarItemInfo();
            BindingOperations.SetBinding(taskbarItemInfo, TaskbarItemInfo.DescriptionProperty, new Binding("Title"));
            BindingOperations.SetBinding(taskbarItemInfo, TaskbarItemInfo.ProgressStateProperty, new Binding("ProgressState"));
            BindingOperations.SetBinding(taskbarItemInfo, TaskbarItemInfo.ProgressValueProperty, new Binding("ProgressValue"));
            this.TaskbarItemInfo = taskbarItemInfo;

            this.SetKeyBinding(Key.F5, "RefreshCommand");
            this.SetEventBinding("Loaded", "StartupCommand");
            this.SetEventBinding("Closing", "ClosingCommand");
        }

        private void SetKeyBinding(Key key, string commandName)
        {
            KeyBinding keyBinding = new KeyBinding() { Key = key };
            BindingOperations.SetBinding(keyBinding, KeyBinding.CommandProperty, new Binding(commandName));
            InputBindings.Add(keyBinding);
        }

        private void SetEventBinding(string eventName, string commandName, bool passEventArgsToCommand = false)
        {
            System.Windows.Interactivity.EventTrigger trigger = new System.Windows.Interactivity.EventTrigger(eventName);
            EventToCommand action = new EventToCommand() { PassEventArgsToCommand = passEventArgsToCommand };
            BindingOperations.SetBinding(action, EventToCommand.CommandProperty, new Binding(commandName));
            trigger.Actions.Add(action);
            Interaction.GetTriggers(this).Add(trigger);
        }
    }
}
