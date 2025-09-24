
using System;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace task_project;

public sealed partial class Group : UserControl
{
    private StackPanel panel;
    private Grid grid;
    private ListView listView;
    private TextBlock groupTitle;
    private AppBarButton openAllButton;
    private AppBarButton addButton;
    private AppBarButton deleteButton;
    
    public event EventHandler? DeleteRequested;

    private static int groupnr = 1;

    public Group()
    {
        panel = new StackPanel
        {
            Margin = new Thickness(10),
            BorderThickness = new Thickness(1),
            BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.WhiteSmoke)
        };
        
        CreateGrid();
        panel.Children.Add(grid);
        
        listView = new ListView
        {
            Height = 150
        };
        panel.Children.Add(listView);
        
        this.Content = panel;
    }
    
    private void CreateGrid()
    {
        grid = new Grid();
            
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        for (int i = 0; i < 3; i++)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        }

        CreateTitle();

        Grid.SetColumn(groupTitle, 0);
        grid.Children.Add(groupTitle);

        CreateButtons();
        
        Grid.SetColumn(openAllButton, 1);
        Grid.SetColumn(addButton, 2);
        Grid.SetColumn(deleteButton, 3);
        
        grid.Children.Add(openAllButton);
        grid.Children.Add(addButton);
        grid.Children.Add(deleteButton);
    }

    private void CreateTitle()
    {
        groupTitle = new TextBlock
        {
            Text = $"New Group {groupnr++}",
            Margin = new Thickness(10, 10, 4, 10),
            TextAlignment = TextAlignment.Center,
            TextTrimming = TextTrimming.CharacterEllipsis,
            VerticalAlignment = VerticalAlignment.Center
        };
    }

    private void CreateButtons()
    {
        openAllButton = new AppBarButton
        {
            Icon = new FontIcon {Glyph = "\uE713"},
            VerticalAlignment = VerticalAlignment.Center,
        };
        
        addButton = new AppBarButton
        {
            Icon = new SymbolIcon(Symbol.Add),
            VerticalAlignment = VerticalAlignment.Center,
        };
        
        deleteButton = new AppBarButton
        {
            Icon = new SymbolIcon(Symbol.Delete),
            VerticalAlignment = VerticalAlignment.Center,
        };
        
        openAllButton.Click += ButtonClick;
        addButton.Click += ButtonClick;
        deleteButton.Click += DeleteClick;
    }
    
    void ButtonClick(object sender, RoutedEventArgs e)
    {
        var clickedButton = sender as AppBarButton;
        if (clickedButton != null)
        {
            clickedButton.Label = "unc";
        }
    }

    void DeleteClick(object sender, RoutedEventArgs e)
    {
        DeleteRequested?.Invoke(this, EventArgs.Empty);       
    }

    void AddClick(object sender, RoutedEventArgs e)
    {
        
    }
}