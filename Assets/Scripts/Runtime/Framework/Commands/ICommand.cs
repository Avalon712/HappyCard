namespace HappyCard
{
    /// <summary>
    /// 如果实现此接口的是结构体，请不要进行Equals()操作，或者显示实现Equals()函数
    /// </summary>
    public interface ICommand
    {
        void Execute();
    }
}
