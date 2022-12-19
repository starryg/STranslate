﻿using STranslate.Utils;
using STranslate.ViewModel;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace STranslate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainVM vm;
        public MainWindow()
        {
            InitializeComponent();

            vm = (MainVM)DataContext;
        }

        /// <summary>
        /// 显示主窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenMainWin_Click(object sender, EventArgs e)
        {
            this.Show();
            this.Activate();
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        /// <summary>
        /// 软件运行时快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //最小化 Esc
            if (e.Key == Key.Escape)
            {
                this.Hide();
                vm.InputTxt = string.Empty;
                vm.OutputTxt = string.Empty;
            }
            //退出 Ctrl+Shift+Q
            if (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control)
                && e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Shift)
                && e.Key == Key.Q)
            {
                Environment.Exit(0);
            }
#if false
            //置顶/取消置顶 Ctrl+T
            if (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.T)
            {
                Topmost = Topmost != true;
                Opacity = Topmost ? 1 : 0.9;
            }
            //缩小 Ctrl+[
            if (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.OemOpenBrackets)
            {
                if (Width < 245)
                {
                    return;
                }
                Width /= 1.2;
                Height /= 1.2;
            }
            //放大 Ctrl+]
            if (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.OemCloseBrackets)
            {
                if (Width > 600)
                {
                    return;
                }
                Width *= 1.2;
                Height *= 1.2;
            }
            //恢复界面大小 Ctrl+P
            if (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.P)
            {
                Width = 400;
                Height = 450;
            }
#endif
        }

        /// <summary>
        /// 监听全局快捷键
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            IntPtr handle = new WindowInteropHelper(this).Handle;
            HotKeysUtil.RegisterHotKey(handle);

            HwndSource source = HwndSource.FromHwnd(handle);
            source.AddHook(WndProc);
        }
        /// <summary>
        /// 热键的功能
        /// </summary>
        /// <param name="m"></param>
        protected IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handle)
        {
            switch (msg)
            {
                case 0x0312: //这个是window消息定义的 注册的热键消息
                    //Console.WriteLine(wParam.ToString());
                    if (wParam.ToString().Equals(HotKeysUtil.InputTranslateId + ""))
                    {
                        this.InputTranslateMenuItem_Click(null, null);
                    }
                    else if (wParam.ToString().Equals(HotKeysUtil.CrosswordTranslateId + ""))
                    {
                        this.CrossWordTranslateMenuItem_Click(null, null);
                    }
                    else if (wParam.ToString().Equals(HotKeysUtil.ScreenShotTranslateId + ""))
                    {
                        this.ScreenshotTranslateMenuItem_Click(null, null);
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        /// <summary>
        /// 非激活窗口则隐藏起来
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// 清空输入输出框
        /// </summary>
        private void ClearTextBox()
        {
            vm.InputTxt = string.Empty;
            vm.OutputTxt = string.Empty;

        }

        /// <summary>
        /// 输入翻译
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputTranslateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ClearTextBox();
            OpenMainWin_Click(null, null);
            TextBoxInput.Focus();
        }

        /// <summary>
        /// 划词翻译
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CrossWordTranslateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ClearTextBox();
            var sentence = GetWords.Get();
            this.Show();
            this.Activate();
            this.TextBoxInput.Text = sentence.Trim();
            _ = vm.Translate();
        }

        /// <summary>
        /// 截图翻译
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScreenshotTranslateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("开发中");
        }
    }
}