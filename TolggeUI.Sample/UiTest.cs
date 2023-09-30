﻿using TolggeUI;
using TolggeUI.Components;

namespace Sample;

public class UiTest : Component
{
    public override RenderObject Build()
    {
        return new Div
            {
                new TextBoxComponent(),
                new CheckboxComponent(),
                new DropDownComponent()
            }
            .Color(50, 50, 50)
            .MAlign(MAlign.FlexStart)
            .XAlign(XAlign.FlexStart)
            .Gap(30)
            .Padding(30);
    }
}