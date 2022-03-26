using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poc.Entity {
    public class SerialVersionConverter {
        private static byte[]? _EmptySerialVersion;
        public static byte[] GetEmptySerialVersion() => (_EmptySerialVersion ??= (new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }));

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
                return ((ulong)bigEndianBinary[0] << 56) |
                       ((ulong)bigEndianBinary[1] << 48) |
                       ((ulong)bigEndianBinary[2] << 40) |
                       ((ulong)bigEndianBinary[3] << 32) |
                       ((ulong)bigEndianBinary[4] << 24) |
                       ((ulong)bigEndianBinary[5] << 16) |
                       ((ulong)bigEndianBinary[6] << 8) |
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
            (byte)(value)
        };
        }
    }
    public class TodoRepository {
        public readonly PocRepository Repository;
        private readonly SerialVersionConverter _SerialVersionConverter;

        public TodoRepository(PocRepository pocRepository) {
            this.Repository = pocRepository;
            this._SerialVersionConverter = new SerialVersionConverter();
        }

        [return: NotNullIfNotNull("e")]
        private TodoItem? ConvertFromEntity(TodoEntity? e) {
            if (e is null) {
                return null;
            } else {
                return new TodoItem() {
                    Id = e.Id,
                    Title = e.Title,
                    Done = e.Done,
                    CreatedAt = e.CreatedAt,
                    ModifiedAt = e.ModifiedAt,
                    SerialVersion = this._SerialVersionConverter.ConvertFromEntity(e.SerialVersion)
                };
            }
        }

        public async Task<List<TodoItem>> GetList() {
            var resultEntity = await this.Repository.Context.Todo.ToListAsync();
            return resultEntity.Select(e => this.ConvertFromEntity(e)).ToList();
        }

        public async Task<TodoItem?> GetItem(Guid id) {
            var resultEntity = await this.Repository.Context.Todo.FindAsync(id);
            return this.ConvertFromEntity(resultEntity);
        }
    }
}
