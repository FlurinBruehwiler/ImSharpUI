﻿using ImSharpUISample.UiElements;
using static ImSharpUISample.Ui;

namespace ImSharpUISample;

public class Sample
{
    private bool _checkboxChecked;
    private bool _isExpanded;
    private List<string> _options = new()
    {
        "Henry",
        "Courdin",
        "Random",
        "Card",
        "Index"
    };
    private string? _selectedOption;

    private readonly List<string> _items = Enumerable.Range(0, 60).Select(x => x.ToString()).ToList();

    public void Build()
    {
        // DivStart().Color(255, 0, 0).Padding(10).Gap(10);
        //     DivStart().Color(255, 255, 0).Radius(10).Padding(10);
        //
        //     DivEnd();
        //     DivStart().Color(0, 255, 0).Radius(10).Padding(10);
        //         DivStart().Color(0, 255, 255).Radius(10);
        //
        //         DivEnd();
        //     DivEnd();
        // DivEnd();
        //
        //
        // return;

        DivStart().Color(255, 0, 0).Dir(Dir.Horizontal).Padding(10);
            DivStart().Gap(10);
                DivStart(out var div).Color(0, 255, 0).Radius(20);
                    if (div.IsHovered)
                        div.Color(0, 200, 0);


                DivEnd();
                DivStart().Color(255, 255, 0).Padding(20).Gap(10);
                    Checkbox();
                    DropDown();
                DivEnd();
            DivEnd();
            DivStart().Scroll().Gap(5).Padding(5).BorderColor(0, 0,0).BorderWidth(2);
                foreach (var x in _items)
                {
                    DivStart(out var itemDiv, x).Height(20).Color(10, 200, 100).Radius(5).Padding(2);
                        if (itemDiv.IsHovered)
                            itemDiv.Color(200, 210, 20);
                        Text(x).VAlign(TextAlign.Center);
                    DivEnd();
                }
            DivEnd();
        DivEnd();
    }

    private void Checkbox()
    {
        DivStart(out var div)
            .Width(20)
            .BorderColor(200, 200, 200, 100)
            .Color(40, 40, 40)
            .BorderWidth(1)
            .Radius(4)
            .Height(20);

            if (div.Clicked)
                _checkboxChecked = !_checkboxChecked;
            if(_checkboxChecked)
                SvgImage("check.svg");

        DivEnd();
    }

    private void DropDown()
    {
        DivStart().Width(200).XAlign(XAlign.Center).Height(35).Color(40, 40, 40).BorderWidth(1).BorderColor(200, 200, 200, 100).Radius(5);

            DivStart(out var outerDiv).Height(35).PaddingLeft(10).PaddingRight(5).Dir(Dir.Horizontal);

                Text(_selectedOption ?? string.Empty).Size(18).VAlign(TextAlign.Center);

                DivStart(out var innerDiv).Width(35).Height(35);
                    SvgImage("expand.svg");
                DivEnd();

            DivEnd();

            if (_isExpanded)
            {
                DivStart().Height(5 * 35).BorderWidth(1).BorderColor(200, 200, 200, 100).Radius(5).Color(40, 40, 40).Absolute(top: 40);

                    foreach (var option in _options)
                    {
                        DivStart(out var optionDiv, option).PaddingLeft(10).Radius(5).Color(40, 40, 40);

                            if (optionDiv.IsHovered)
                                optionDiv.Color(50, 50, 50);

                            if (optionDiv.Clicked)
                                _selectedOption = option;

                            Text(option).VAlign(TextAlign.Center).Size(18);

                        DivEnd();
                    }

                DivEnd();
            }

            _isExpanded = outerDiv.IsActive || innerDiv.IsActive;

        DivEnd();
    }
}