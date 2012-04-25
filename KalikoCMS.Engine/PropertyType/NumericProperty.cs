namespace KalikoCMS.PropertyType {
    using KalikoCMS.Attributes;
    using KalikoCMS.Core;
    using KalikoCMS.Serialization;

    [PropertyType("C40B0CB0-DB98-4A59-A000-037EC5189DF0", "Numeric", "Numeric", "~/Admin/Content/PropertyType/NumericPropertyEditor.ascx")]
    public class NumericProperty : PropertyData {
        private static readonly int EmptyHashCode = string.Empty.GetHashCode();
        private int? _cachedHashCode;
        private int _value;

        public NumericProperty() {
        }

        public NumericProperty(int value) {
            Value = value;
        }

        public NumericProperty(string value) {
            int number;
            if(int.TryParse(value, out number)) {
                Value = number;
            }
        }

        protected override string StringValue {
            get {
                return Value.ToString();
            }
        }

        public int Value {
            get {
                return _value;
            }
            set {
                _value = value;
                ValueSet = true;
            }
        }

        public bool ValueSet { get; set; }

        protected override PropertyData DeserializeFromJson(string data) {
            return JsonSerialization.DeserializeJson<NumericProperty>(data);
        }

        public override int GetHashCode() {
            return (int)(_cachedHashCode ?? (_cachedHashCode = CalculateHashCode()));
        }

        private int CalculateHashCode() {
            return Value.GetHashCode();
        }
    }
}
