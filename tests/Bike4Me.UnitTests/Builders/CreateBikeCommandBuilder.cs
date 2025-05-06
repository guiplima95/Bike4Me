using Bike4Me.Application.Bikes.Commands;

namespace Bike4Me.UnitTests.Builders;

public sealed class CreateBikeCommandBuilder
{
    private string _plate = "ABC1D23";

    private string _color = "Black";
    private string _modelName = "CB300";
    private string _modelManufacturer = "Honda";
    private int _modelYear = DateTime.UtcNow.Year;
    private string _engineCapacity = "300cc";

    public CreateBikeCommandBuilder()
    { }

    public CreateBikeCommandBuilder WithPlate(string plate)
    {
        _plate = plate;
        return this;
    }

    public CreateBikeCommandBuilder WithColor(string color)
    {
        _color = color;
        return this;
    }

    public CreateBikeCommandBuilder WithModelName(string modelName)
    {
        _modelName = modelName;
        return this;
    }

    public CreateBikeCommandBuilder WithManufacturer(string manufacturer)
    {
        _modelManufacturer = manufacturer;
        return this;
    }

    public CreateBikeCommandBuilder WithYear(int year)
    {
        _modelYear = year;
        return this;
    }

    public CreateBikeCommandBuilder WithEngineCapacity(string capacity)
    {
        _engineCapacity = capacity;
        return this;
    }

    public CreateBikeCommand Build()
    {
        return new CreateBikeCommand(
            Plate: _plate,
            Color: _color,
            ModelName: _modelName,
            ModelManufacturer: _modelManufacturer,
            ModelYear: _modelYear,
            ModelEngineCapacity: _engineCapacity);
    }

    public static implicit operator CreateBikeCommand(CreateBikeCommandBuilder builder) => builder.Build();
}