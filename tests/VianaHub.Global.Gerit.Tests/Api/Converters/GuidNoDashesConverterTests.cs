using System;
using System.Text;
using System.Text.Json;
using FizzWare.NBuilder;
using VianaHub.Global.Gerit.Api.Converters;
using Xunit;

namespace VianaHub.Global.Gerit.Tests.Api.Converters
{
    public class GuidNoDashesConverterTests
    {
        private class SimpleDto { public Guid Id { get; set; } }

        [Fact(DisplayName = "GuidNoDashesConverter - Write escreve Guid sem hífens")]
        [Trait("Layer", "Api")]
        public void Write_WritesGuidWithoutDashes()
        {
            var dto = Builder<SimpleDto>.CreateNew().Build();
            if (dto.Id == Guid.Empty) dto.Id = Guid.NewGuid();

            var converter = new GuidNoDashesConverter();

            using var ms = new MemoryStream();
            using (var writer = new Utf8JsonWriter(ms))
            {
                converter.Write(writer, dto.Id, new JsonSerializerOptions());
                writer.Flush();
            }

            var json = Encoding.UTF8.GetString(ms.ToArray());

            Assert.Equal($"\"{dto.Id.ToString("N")}\"", json);
        }

        [Theory(DisplayName = "GuidNoDashesConverter - Read aceita formatos com e sem hífens")]
        [InlineData("f3890433-c100-4470-a322-0a14f2868782")]
        [InlineData("f3890433c1004470a3220a14f2868782")]
        [Trait("Layer", "Api")]
        public void Read_ReadsGuidWithOrWithoutDashes(string value)
        {
            var expected = Guid.Parse("f3890433-c100-4470-a322-0a14f2868782");
            var json = $"\"{value}\"";
            var bytes = Encoding.UTF8.GetBytes(json);
            var reader = new Utf8JsonReader(bytes);
            reader.Read(); // move to string token

            var converter = new GuidNoDashesConverter();

            var result = converter.Read(ref reader, typeof(Guid), new JsonSerializerOptions());

            Assert.Equal(expected, result);
        }
    }
}
