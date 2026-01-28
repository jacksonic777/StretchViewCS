unit UfrmHelp;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls;

type
  TfrmHelp = class(TForm)
    GroupBox1: TGroupBox;
    Label3: TLabel;
    Label2: TLabel;
    GroupBox2: TGroupBox;
    Label4: TLabel;
    Label5: TLabel;
    Label6: TLabel;
    Label7: TLabel;
    GroupBox3: TGroupBox;
    lblFix: TLabel;
    btnOK: TButton;
    procedure FormCreate(Sender: TObject);
    procedure btnOKClick(Sender: TObject);
  private
    { Private 宣言 }
  public
    { Public 宣言 }
  end;

var
  frmHelp: TfrmHelp;

implementation

{$R *.dfm}

procedure TfrmHelp.FormCreate(Sender: TObject);
begin
        lblFix.caption :=
          '　拡大・回転した表示ウィンドウに描き込むと、'
          + #13 + '映しているウィンドウに直接反映される機能です。'
          + #13 + '反映させたい領域を表示ウィンドウに移した状態で、'
          + #13 + '「Ctrl+E」を押すと、領域が決定され、'
          + #13 + '書き込めるようになります。'
          + #13 + 'モードを解除するときは、再度「Ctrl+E」を押すか、'
          + #13 + 'メニューからチェックをはずしてください。';
end;

procedure TfrmHelp.btnOKClick(Sender: TObject);
begin
        frmHelp.Close;
end;

end.
