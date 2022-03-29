namespace Poc.Repository {
    public class SerialVersionConverter {
        private static byte[]? _EmptySerialVersion;
        public static byte[] GetEmptySerialVersion() => _EmptySerialVersion ??= (new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });

        private static SerialVersionConverter? _Instance;
        public static SerialVersionConverter GetInstance() => _Instance ??= (new SerialVersionConverter());

        public string ConvertFromEntity(byte[] serialVersion) {
            var ulSerialVersion = ConvertBigEndianToUInt64(serialVersion);
            return ulSerialVersion.ToString("x16");
        }

        public byte[] ConvertToEntity(string serialVersion) {
            if (ulong.TryParse(
                serialVersion,
                System.Globalization.NumberStyles.HexNumber,
                System.Globalization.CultureInfo.InvariantCulture,
                out var result)) {
                return ConvertUInt64ToBigEndian(result);
            } else {
                return GetEmptySerialVersion();
            }
        }

        public static ulong ConvertBigEndianToUInt64(byte[] bigEndianBinary) {
            if (bigEndianBinary.Length == 8) {
                return (ulong)bigEndianBinary[0] << 56 |
                       (ulong)bigEndianBinary[1] << 48 |
                       (ulong)bigEndianBinary[2] << 40 |
                       (ulong)bigEndianBinary[3] << 32 |
                       (ulong)bigEndianBinary[4] << 24 |
                       (ulong)bigEndianBinary[5] << 16 |
                       (ulong)bigEndianBinary[6] << 8 |
                               bigEndianBinary[7];
            } else {
                return 0;
            }
        }

        public static byte[] ConvertUInt64ToBigEndian(ulong value) {
            return new[]
            {
            (byte)(value >> 56),
            (byte)(value >> 48),
            (byte)(value >> 40),
            (byte)(value >> 32),
            (byte)(value >> 24),
            (byte)(value >> 16),
            (byte)(value >> 8),
            (byte)value
        };
        }

        public bool EqualsSerialVersion(byte[] vs1, byte[] vs2) {
            if (vs1.Length == vs2.Length) {
                for (int i = 0; i < vs1.Length; i++) {
                    if (vs1[i] != vs2[i]) {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
