using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UI
{
    public enum NumberBoxType { Float, Fixed, Integer }

    /// <summary>
    /// Logique d'interaction pour NumberBox.xaml
    /// </summary>
    public partial class NumberBox : UserControl
    {
        public event EventHandler ValueChanged;

        public NumberBox()
        {
            InitializeComponent();
            Value = 0.0D;
            Min = double.NaN;
            Max = double.NaN;
            TextAlignment = HorizontalAlignment.Center;
            NumberStyle = NumberStyles.Float | NumberStyles.AllowThousands;
            NumberFormat = string.Empty;
        }

        /// <summary>
        /// NumberStyles to use. By defaut, uses NumberStyles.Float and NumberStyles.AllowThousands
        /// </summary>
        public NumberStyles NumberStyle
        {
            get { return _numberStyle; }
            set
            {
                _numberStyle = value;
                ValidateText();
            }
        }
        private NumberStyles _numberStyle;

        /// <summary>
        /// Specify a specific number format. If ExponentMode is set to true, this is ignored.
        /// </summary>
        public string NumberFormat
        {
            get { return _numberFormat; }
            set
            {
                _numberFormat = value;
                ValidateText();
            }
        }
        private string _numberFormat;

        /// <summary>
        /// Number type of the value, either Float (double), Fixed (decimal), or Integer (long). Default is Float. Assigns the dynamic type of Value.
        /// </summary>
        public NumberBoxType NumberType
        {
            get { return _numberType; }

            set
            {
                _numberType = value;

                //Resets the values to their corresponding data type
                Value = Value;
                Min = Min;
                Max = Max;
            }
        }
        private NumberBoxType _numberType;

        /// <summary>
        /// Used to determine if the Value is set
        /// </summary>
        public bool IsValueSet { get; private set; }
        /// <summary>
        /// The value. Dynamic type, either double (default), decimal or long based on NumberType. Can be made to be unset by setting it to double.NaN, regardless of NumberType.
        /// </summary>
        public dynamic Value
        {
            get
            {
                if (!IsValueSet) return double.NaN;
                switch (NumberType)
                {
                    default:
                    case NumberBoxType.Float:
                        return (double)_value;

                    case NumberBoxType.Fixed:
                        if (_value.GetType() == typeof(decimal))
                            return (decimal)_value;
                        else
                        {
                            if ((double)_value > (double)decimal.MaxValue) return decimal.MaxValue;
                            else if ((double)_value < (double)decimal.MinValue) return decimal.MinValue;
                            else return (decimal)_value;
                        }

                    case NumberBoxType.Integer:
                        if (_value.GetType() == typeof(long))
                            return (long)_value;
                        else
                        {
                            if ((double)_value > (double)long.MaxValue) return long.MaxValue;
                            else if ((double)_value < (double)long.MinValue) return long.MinValue;
                            else return (long)_value;
                        }
                }
            }

            set
            {
                if (value.GetType() != typeof(string))
                {
                    if (value.GetType() == typeof(double) && double.IsNaN(value))
                    {
                        _value = double.NaN;
                        IsValueSet = false;
                    }
                    else
                    {
                        switch (NumberType)
                        {
                            default:
                            case NumberBoxType.Float:
                                _value = (double)value;
                                break;
                            case NumberBoxType.Fixed:
                                _value = (decimal)value;
                                break;
                            case NumberBoxType.Integer:
                                _value = (long)value;
                                break;
                        }
                        IsValueSet = true;
                    }
                }
                ValidateValue();

                if (ValueChanged != null) ValueChanged(this, new EventArgs());
            }
        }
        private dynamic _value;

        /// <summary>
        /// Used to determine whether the Min value is set.
        /// </summary>
        public bool IsMinSet { get; private set; }
        /// <summary>
        /// Minimum allowed valued, inclusive. Set to double.NaN is unset.
        /// </summary>
        public dynamic Min
        {
            get
            {
                if (!IsMinSet) return double.NaN;
                switch (NumberType)
                {
                    default:
                    case NumberBoxType.Float:
                        return (double)_min;

                    case NumberBoxType.Fixed:
                        if (_min.GetType() == typeof(decimal))
                            return (decimal)_min;
                        else
                        {
                            if ((double)_min > (double)decimal.MaxValue) return decimal.MaxValue;
                            else if ((double)_min < (double)decimal.MinValue) return decimal.MinValue;
                            else return (decimal)_min;
                        }

                    case NumberBoxType.Integer:
                        if (_min.GetType() == typeof(long))
                            return (long)_min;
                        else
                        {
                            if ((double)_min > (double)long.MaxValue) return long.MaxValue;
                            else if ((double)_min < (double)long.MinValue) return long.MinValue;
                            else return (long)_min;
                        }
                }
            }
            set
            {
                //Unset Min
                if (value.GetType() == typeof(double) && double.IsNaN(value))
                {
                    switch (NumberType)
                    {
                        default:
                        case NumberBoxType.Float:
                            _min = double.MinValue;
                            break;
                        case NumberBoxType.Fixed:
                            _min = decimal.MinValue;
                            break;
                        case NumberBoxType.Integer:
                            _min = long.MinValue;
                            break;
                    }
                    IsMinSet = false;
                }
                //Set Min
                else
                {
                    switch (NumberType)
                    {
                        default:
                        case NumberBoxType.Float:
                            _min = (double)value;
                            break;
                        case NumberBoxType.Fixed:
                            _min = (decimal)value;
                            break;
                        case NumberBoxType.Integer:
                            _min = (long)value;
                            break;
                    }
                    IsMinSet = true;
                }
                UpdateToolTip();
                ValidateValue();
            }
        }
        private dynamic _min;

        /// <summary>
        /// Used to determine whether the Max value is set.
        /// </summary>
        public bool IsMaxSet { get; private set; }
        /// <summary>
        /// Maximum allowed valued, inclusive. Set to double.NaN is unset.
        /// </summary>
        public dynamic Max
        {
            get
            {
                if (!IsMaxSet) return double.NaN;
                switch (NumberType)
                {
                    default:
                    case NumberBoxType.Float:
                        return (double)_max;

                    case NumberBoxType.Fixed:
                        if (_max.GetType() == typeof(decimal))
                            return (decimal)_max;
                        else
                        {
                            if ((double)_max > (double)decimal.MaxValue) return decimal.MaxValue;
                            else if ((double)_max < (double)decimal.MinValue) return decimal.MinValue;
                            else return (decimal)_max;
                        }

                    case NumberBoxType.Integer:
                        if (_max.GetType() == typeof(long))
                            return (long)_max;
                        else
                        {
                            if ((double)_max > (double)long.MaxValue) return long.MaxValue;
                            else if ((double)_max < (double)long.MinValue) return long.MinValue;
                            else return (long)_max;
                        }
                }
            }
            set
            {
                //Unset Max
                if (value.GetType() == typeof(double) && double.IsNaN(value))
                {
                    switch (NumberType)
                    {
                        default:
                        case NumberBoxType.Float:
                            _max = double.MaxValue;
                            break;
                        case NumberBoxType.Fixed:
                            _max = decimal.MaxValue;
                            break;
                        case NumberBoxType.Integer:
                            _max = long.MaxValue;
                            break;
                    }
                    IsMaxSet = false;
                }
                //Set Max
                else
                {
                    switch (NumberType)
                    {
                        default:
                        case NumberBoxType.Float:
                            _max = (double)value;
                            break;
                        case NumberBoxType.Fixed:
                            _max = (decimal)value;
                            break;
                        case NumberBoxType.Integer:
                            _max = (long)value;
                            break;
                    }
                    IsMaxSet = true;
                }
                UpdateToolTip();
                ValidateValue();
            }

        }
        private dynamic _max;

        /// <summary>
        /// Validating value for min and max
        /// </summary>
        private void ValidateValue()
        {
            if (!IsValueSet) return;

            if (IsMinSet && IsMaxSet)
            {
                if (Min > Max)
                {
                    var tmp = Min;
                    Min = Max;
                    Max = tmp;
                }
            }

            if (IsMinSet)
            {
                if (Value < Min) Value = Min;
            }

            if (IsMaxSet)
            {
                if (Value > Max) Value = Max;
            }

            ResetToLastValue();
        }

        /// <summary>
        /// Validating input value
        /// </summary>
        private void ValidateText()
        {
            bool canParse;

            switch (NumberType)
            {
                default:
                case NumberBoxType.Float:
                    double resultD;
                    canParse = double.TryParse(Container.Text, NumberStyle, CultureInfo.CurrentCulture, out resultD);
                    if (canParse) Value = resultD;
                    else ResetToLastValue();
                    break;

                case NumberBoxType.Fixed:
                    decimal resultM;
                    canParse = decimal.TryParse(Container.Text, NumberStyle, CultureInfo.CurrentCulture, out resultM);
                    if (canParse) Value = resultM;
                    else ResetToLastValue();
                    break;

                case NumberBoxType.Integer:
                    long resultL;
                    canParse = long.TryParse(Container.Text, NumberStyle, CultureInfo.CurrentCulture, out resultL);
                    if (canParse) Value = resultL;
                    else ResetToLastValue();
                    break;
            }
        }

        /// <summary>
        /// Updates the text of the underlying box to display the validated value
        /// </summary>
        private void ResetToLastValue()
        {
            string text;
            if (NumberFormat == string.Empty)
                text = Value.ToString();
            else
                text = Value.ToString(NumberFormat);

            Container.Text = text;
            Container.CaretIndex = Container.Text.Length;
        }

        /// <summary>
        /// Updates the text of the tool tip
        /// </summary>
        private void UpdateToolTip()
        {
            if (!IsMinSet && !IsMaxSet)
            {
                Container.ToolTip = null;
                return;
            }

            string tiptext = string.Empty;

            if (IsMinSet) tiptext += "Min : " + Min;
            if (IsMinSet && IsMaxSet) tiptext += "\n";
            if (IsMaxSet) tiptext += "Max : " + Max;

            Container.ToolTip = tiptext;
        }

        /// <summary>
        /// Horizontal alignment of text.
        /// </summary>
        public HorizontalAlignment TextAlignment
        {
            get { return Container.HorizontalContentAlignment; }
            set { Container.HorizontalContentAlignment = value; }
        }

        /// <summary>
        /// TabIndex for the underlying control.
        /// </summary>
        public new int TabIndex
        {
            get { return Container.TabIndex; }
            set { Container.TabIndex = value; }
        }

        /// <summary>
        /// IsReadOnly for the underlying control.
        /// </summary>
        public bool IsReadOnly
        {
            get { return Container.IsReadOnly; }
            set { Container.IsReadOnly = value; }
        }

        #region Insertion of Decimal Separator
        /// <summary>
        /// FLAG to be used when NumPad Decimal key is pressed.
        /// </summary>
        private bool FLAG_NumPadDecimal;

        /// <summary>
        /// On KeyDown event, if the key is NumPad Decimal or Space, the corresponding FLAG is set.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Container_KeyDown(object sender, KeyEventArgs e)
        {
            if (IsReadOnly) return;

            if (e.Key == Key.Decimal) FLAG_NumPadDecimal = true;
        }

        /// <summary>
        /// On KeyUp event, FLAG is unset. Number validation is triggered if the Enter key is released.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Container_KeyUp(object sender, KeyEventArgs e)
        {
            if (IsReadOnly) return;

            FLAG_NumPadDecimal = false;
            if (e.Key == Key.Enter) ValidateText();
        }

        /// <summary>
        /// On PreviewTextInput event, if the FLAG is set, prevents the normal keystroke and inserts it the with NumberDecimalSeparator of the CurrentCulture instead.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Container_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsReadOnly) return;

            if (FLAG_NumPadDecimal)
            {
                FLAG_NumPadDecimal = false;
                e.Handled = true;
                int insertIndex = Container.CaretIndex;
                string text = Container.Text;
                Container.Text = text.Substring(0, insertIndex) + CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + text.Substring(insertIndex);
                Container.CaretIndex = insertIndex + CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.Length;
            }
        }
        #endregion

        /// <summary>
        /// On LostFocus event, validate the input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Container_LostFocus(object sender, RoutedEventArgs e)
        {
            ValidateText();
        }

        /// <summary>
        /// On GotKeyboardFocus event, force focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Container_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ForceFocus();
        }

        /// <summary>
        /// On GotFocus, force focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            ForceFocus();
        }

        /// <summary>
        /// Force to focus on the underlying box, and select the text
        /// </summary>
        public void ForceFocus()
        {
            Container.Focus();
            Container.SelectAll();
        }
    }
}
