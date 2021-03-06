﻿// <copyright file="CitiesComboBoxItem.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace RealTime.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using ColossalFramework.UI;
    using ICities;
    using RealTime.Localization;

    /// <summary>A check box item.</summary>
    internal sealed class CitiesComboBoxItem : CitiesViewItem<UIDropDown, int>
    {
        private const string LabelName = "Label";

        private readonly IEnumerable<string> itemIds;

        /// <summary>Initializes a new instance of the <see cref="CitiesComboBoxItem"/> class.</summary>
        /// <param name="uiHelper">The game's UI helper reference.</param>
        /// <param name="id">The view item's unique ID.</param>
        /// <param name="property">
        /// The property description that specifies the target property where to store the value.
        /// </param>
        /// <param name="config">The configuration storage object for the value.</param>
        /// <param name="itemIds">An ordered collection of the combo box item IDs.</param>
        /// <exception cref="ArgumentNullException">Thrown when any argument is null.</exception>
        /// <exception cref="ArgumentException">
        /// thrown when the <paramref name="id"/> is an empty string.
        /// </exception>
        public CitiesComboBoxItem(UIHelperBase uiHelper, string id, PropertyInfo property, object config, IEnumerable<string> itemIds)
            : base(uiHelper, id, property, config)
        {
            this.itemIds = itemIds ?? throw new ArgumentNullException(nameof(itemIds));

            UIComponent.width = 320;

            var parentPanel = UIComponent.parent as UIPanel;
            if (parentPanel != null)
            {
                parentPanel.autoLayoutDirection = LayoutDirection.Horizontal;
                parentPanel.autoSize = true;
            }
        }

        /// <summary>Translates this view item using the specified localization provider.</summary>
        /// <param name="localizationProvider">The localization provider to use for translation.</param>
        /// <exception cref="ArgumentNullException">Thrown when the argument is null.</exception>
        public override void Translate(ILocalizationProvider localizationProvider)
        {
            if (localizationProvider == null)
            {
                throw new ArgumentNullException(nameof(localizationProvider));
            }

            UIComponent.tooltip = localizationProvider.Translate(UIComponent.name + TranslationKeys.Tooltip);

            UILabel label = UIComponent.parent?.Find<UILabel>(LabelName);
            if (label != null)
            {
                label.text = localizationProvider.Translate(UIComponent.name);
                label.wordWrap = true;
                label.autoSize = false;
                label.autoHeight = true;
                label.width = 240;
            }

            UIComponent.items = itemIds.Select(item => localizationProvider.Translate($"{UIComponent.name}.{item}")).ToArray();
            UIComponent.selectedIndex = Value;
        }

        /// <summary>
        /// Refreshes this view item by re-fetching its value from the bound configuration property.
        /// </summary>
        public override void Refresh()
        {
            UIComponent.selectedIndex = Value;
        }

        /// <summary>Creates the view item using the provided <see cref="UIHelperBase"/>.</summary>
        /// <param name="uiHelper">The UI helper to use for item creation.</param>
        /// <param name="defaultValue">The item's default value.</param>
        /// <returns>A newly created view item.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the argument is null.</exception>
        protected override UIDropDown CreateItem(UIHelperBase uiHelper, int defaultValue)
        {
            if (uiHelper == null)
            {
                throw new ArgumentNullException(nameof(uiHelper));
            }

            return (UIDropDown)uiHelper.AddDropdown(Constants.Placeholder, new string[0], defaultValue, ValueChanged);
        }
    }
}