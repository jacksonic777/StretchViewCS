program ScaleLenz;

uses
  Forms,
  uMain in 'uMain.pas' {frmMain},
  uIniManager in 'uIniManager.pas',
  uVersion in 'uVersion.pas' {frmVersion};

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TfrmMain, frmMain);
  Application.Run;
end.
