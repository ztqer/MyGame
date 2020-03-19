public interface ICareAboutGlobalChange
{
    void SelectChange(object sender, GlobalChangeController.GlobalChangeEventArgs e);
    void ApplyChange(IMessage m);
}