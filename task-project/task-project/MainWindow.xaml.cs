using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;

namespace task_project
{
    public sealed partial class MainWindow : Window
    {
        private int currentRow = 0;
        private const int MaxColumns = 3;
        
        public MainWindow()
        {
            this.InitializeComponent();

            var scrollViewer = new ScrollViewer
            {
                VerticalScrollMode = ScrollMode.Enabled,
                HorizontalScrollMode = ScrollMode.Disabled,
            };

            var rootGrid = new Grid();
            
            var mainGrid = new Grid();
            for (int i = 0; i < MaxColumns; i++)
            {
                mainGrid.RowDefinitions.Add(new RowDefinition());
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            
            scrollViewer.Content = mainGrid;
            rootGrid.Children.Add(scrollViewer);
            rootGrid.Children.Add(CreateAddGroupButton());
            this.Content = rootGrid;
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

        
        void Button_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as AppBarButton;
            if (clickedButton != null)
            {
                clickedButton.Label = "unc";
            }
        }


        private StackPanel CreateStackPanel()
        {
            var stackPanel = new StackPanel
            {
                Margin = new Thickness(10),
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.WhiteSmoke)
            };
            
            var panelGrid = CreatePanelGrid();
            stackPanel.Children.Add(panelGrid);
            
            var listView = new ListView
            {
                Height = 150
            };
        
            // Add some sample items
            listView.Items.Add(new ListViewItem { Content = "New Item 1" });
            listView.Items.Add(new ListViewItem { Content = "New Item 2" });

            stackPanel.Children.Add(listView);
            
            return stackPanel;
        }
        private Grid CreatePanelGrid()
        {
            var panelGrid = new Grid();
            
            panelGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            for (int i = 0; i < 3; i++)
            {
                panelGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }

            // Create and add the header TextBlock
            var headerText = new TextBlock
            {
                Text = $"New Group {currentRow + 1}",
                Margin = new Thickness(10),
                TextAlignment = TextAlignment.Center
            };
            
            Grid.SetColumn(headerText, 0);
            panelGrid.Children.Add(headerText);

            AppBarButton button1 = new AppBarButton();
            Grid.SetColumn(button1, 1);
            button1.Click += Button_Click;
            FontIcon icon = new FontIcon { Glyph = "\uEA3A" };
            button1.Icon = icon;
            panelGrid.Children.Add(button1);

            AppBarButton button2 = new AppBarButton
            {
                Icon = new SymbolIcon(Symbol.Add)
            };
            Grid.SetColumn(button2, 2);
            button2.Click += Button_Click;
            panelGrid.Children.Add(button2);
            
            AppBarButton button3 = new AppBarButton { Icon = new SymbolIcon(Symbol.Delete) };
            Grid.SetColumn(button3, 3);
            button3.Click += Button_Click;
            panelGrid.Children.Add(button3);
            
            return panelGrid;
        }

        void AddGroup(object sender, RoutedEventArgs e)
        {
            var stackPanel = CreateStackPanel();

            // Find the next available position
            int column = currentRow % MaxColumns;
            int row = currentRow / MaxColumns;

            // Set the Grid position
            Grid.SetRow(stackPanel, row);
            Grid.SetColumn(stackPanel, column);

            // Add the StackPanel to the main grid
            var rootGrid = (Grid)this.Content;
            var scrollViewer = (ScrollViewer)rootGrid.Children[0];
            var mainGrid = (Grid)scrollViewer.Content;
        
            // Check if we need to add a new row definition
            if (row >= mainGrid.RowDefinitions.Count)
            {
                mainGrid.RowDefinitions.Add(new RowDefinition());
            }

            mainGrid.Children.Add(stackPanel);
        
            // Increment the current row counter
            currentRow++;
        }

    }

    
}