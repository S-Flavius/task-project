using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using System.Linq;

namespace task_project
{
    public sealed partial class MainWindow : Window
    {
        private int _currentRow = 0;
        private int _currentColumn = 0;
        private const int MaxColumns = 3;
        
        private readonly Grid _rootGrid;
        private readonly ScrollViewer _scrollViewer;
        private readonly Grid _mainGrid;
        
        public MainWindow()
        {
            this.InitializeComponent();

            _scrollViewer = new ScrollViewer
            {
                VerticalScrollMode = ScrollMode.Enabled,
                HorizontalScrollMode = ScrollMode.Disabled,
            };

            _rootGrid = new Grid();
            
            _mainGrid = new Grid();
            for (int i = 0; i < MaxColumns; i++)
            {
                _mainGrid.RowDefinitions.Add(new RowDefinition());
                _mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            
            _scrollViewer.Content = _mainGrid;
            _rootGrid.Children.Add(_scrollViewer);
            _rootGrid.Children.Add(CreateAddGroupButton());
            this.Content = _rootGrid;
        }
        
        private Button CreateAddGroupButton()
        {
            Button addGroupButton = new Button
            {
                Content = "Add Group",
                Margin = new Thickness(5, 5, 5, 20),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Colors.WhiteSmoke),
                Width = 120,
                Background = new SolidColorBrush(Colors.Black)
            };
            
            addGroupButton.Click += AddGroup;

            ScaleTransform scaleTransform = new ScaleTransform
            {
                ScaleX = 1.5,
                ScaleY = 1.5
            };
            addGroupButton.RenderTransform = scaleTransform;
    
            Canvas.SetZIndex(addGroupButton, 999);

            return addGroupButton;
        }
        
        void AddGroup(object sender, RoutedEventArgs e)
        {
            var group = new Group();
            group.DeleteRequested += OnGroupDeleteRequested;

            // Set the Grid position
            
            Grid.SetRow(group, _currentRow);
            Grid.SetColumn(group, _currentColumn);
            
            _mainGrid.Children.Add(group);
            
            _currentColumn++;
            if (_currentColumn == MaxColumns)
            {
                _currentColumn = 0;
                _currentRow++;
            }
            
            if (_currentRow >= _mainGrid.RowDefinitions.Count)
            {
                _mainGrid.RowDefinitions.Add(new RowDefinition());
            }
        }

        void OnGroupDeleteRequested(object? sender, EventArgs e)
        {
            var group = (Group)sender!;
            var row = Grid.GetRow(group);
            var col = Grid.GetColumn(group);

            group.DeleteRequested -= OnGroupDeleteRequested;
            _mainGrid.Children.Remove(group);
            
            // Reflow remaining children. Iterate the actual type you add (Group), not StackPanel.
            ReflowGrid(row, col);

            // Update your insertion cursor to “previous cell”
            _currentColumn--;
            if (_currentColumn < 0)
            {
                _currentColumn = MaxColumns - 1;
                _currentRow--;
                // Optionally trim extra row if now unused
                if (_currentRow > 3 && _mainGrid.RowDefinitions.Count > 0)
                    _mainGrid.RowDefinitions.RemoveAt(_mainGrid.RowDefinitions.Count - 1);
            }
        }

        public void ReflowGrid(int deletedPanelRow, int deletedPanelColumn)
        {
            foreach (var group in _mainGrid.Children.OfType<Group>())
            {
                var currentRow = Grid.GetRow(group);
                var currentColumn = Grid.GetColumn(group);

                if (currentRow < deletedPanelRow)
                {
                    continue;
                }
                
                if (currentRow == deletedPanelRow)
                {
                    if (currentColumn > deletedPanelColumn)
                    {
                        Grid.SetColumn(group, currentColumn - 1);
                    }
                }

                if (currentRow > deletedPanelRow)
                {
                    if (currentColumn - 1 >= 0)
                    {
                        Grid.SetColumn(group, currentColumn - 1);
                    }
                    else
                    {
                        Grid.SetRow(group, currentRow - 1);
                        Grid.SetColumn(group, MaxColumns - 1);
                    }
                }
            }
        }

    }

    
}