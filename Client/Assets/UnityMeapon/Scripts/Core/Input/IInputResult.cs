
namespace Meapon.Input
{
    public interface IInputResult
    {
        bool IsSuccess { get; }
        object Data { get; }
    }
}
