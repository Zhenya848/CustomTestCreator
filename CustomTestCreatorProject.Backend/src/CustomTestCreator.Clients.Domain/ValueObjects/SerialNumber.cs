using CSharpFunctionalExtensions;
using CustomTestCreator.SharedKernel;

namespace CustomTestCreator.Clients.Domain.ValueObjects;

public record SerialNumber
{
    public static SerialNumber First = new SerialNumber(1);
    public int Value { get; }

    private SerialNumber(int value)
    {
        Value = value;
    }

    public static Result<SerialNumber, Error> Create(int number)
    {
        if (number <= 0)
            return Errors.General.ValueIsInvalid("Serial number");

        return new SerialNumber(number);
    }

    public static implicit operator int(SerialNumber serialNumber) =>
        serialNumber.Value;
}