public interface IMinigame
{
    void StartGame();
    bool IsRunning { get; }
    bool IsFinished { get; }
    bool PlayerCheated { get; }
}
//ve chvíli co minihra implementuje tenhle interface tak ji umi spustit.
//Je to øešení abychom se vyhnuli vypisování každé jedné minihry do NPC scriptu.