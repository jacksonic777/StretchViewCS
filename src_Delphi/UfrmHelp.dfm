object frmHelp: TfrmHelp
  Left = 413
  Top = 317
  BorderStyle = bsDialog
  Caption = #12504#12523#12503
  ClientHeight = 357
  ClientWidth = 332
  Color = clBtnFace
  Font.Charset = SHIFTJIS_CHARSET
  Font.Color = clWindowText
  Font.Height = -12
  Font.Name = #65325#65331' '#65328#12468#12471#12483#12463
  Font.Style = []
  OldCreateOrder = False
  Position = poOwnerFormCenter
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 12
  object Label6: TLabel
    Left = 16
    Top = 8
    Width = 261
    Height = 12
    Caption = #25313#22823#12539#22238#36578#12499#12517#12540#65291#25551#12365#36796#12415#12394#12393#12434#34892#12358#12484#12540#12523#12391#12377#12290
  end
  object Label7: TLabel
    Left = 8
    Top = 304
    Width = 314
    Height = 12
    Caption = #12454#12451#12531#12489#12454#12469#12452#12474#12399#21487#22793#12391#12377#12290#24341#12387#24373#12427#12392#12469#12452#12474#12434#22793#12360#12425#12428#12414#12377
  end
  object GroupBox1: TGroupBox
    Left = 8
    Top = 32
    Width = 297
    Height = 65
    Caption = #12461#12540#12508#12540#12489#12398#25805#20316
    TabOrder = 0
    object Label3: TLabel
      Left = 24
      Top = 24
      Width = 249
      Height = 12
      Caption = #20493#29575' +  '#65306#12288'"Ctrl + A"        '#20493#29575' -  '#65306#12288'"Ctrl + S"'
    end
    object Label2: TLabel
      Left = 16
      Top = 40
      Width = 249
      Height = 12
      Caption = #24038#21491#21453#36578' '#65306#12288'"Ctrl + D"   '#19978#19979#21453#36578' '#65306#12288'"Ctrl + F"'
    end
  end
  object GroupBox2: TGroupBox
    Left = 8
    Top = 240
    Width = 305
    Height = 57
    Caption = #22266#23450#12514#12540#12489#26178#12398#25805#20316
    TabOrder = 1
    object Label4: TLabel
      Left = 16
      Top = 24
      Width = 230
      Height = 12
      Caption = #38936#22495#12398#31227#21205#12288#19978#12408#65306'  Ctrl +'#8593#12288#19979#12408#65306'Ctrl +'#8595' '
    end
    object Label5: TLabel
      Left = 88
      Top = 40
      Width = 162
      Height = 12
      Caption = #24038#12408#65306' Ctrl + '#8592' '#21491#12408#65306' Ctrl + '#8594
    end
  end
  object GroupBox3: TGroupBox
    Left = 8
    Top = 104
    Width = 305
    Height = 129
    Caption = #22266#23450#12514#12540#12489#65288#12371#12387#12385#12391#25551#12365#12371#12415#12514#12540#12489#65289#12392#12399
    TabOrder = 2
    object lblFix: TLabel
      Left = 8
      Top = 21
      Width = 28
      Height = 12
      Caption = 'lblFix'
    end
  end
  object btnOK: TButton
    Left = 96
    Top = 328
    Width = 97
    Height = 25
    Caption = 'OK'
    TabOrder = 3
    OnClick = btnOKClick
  end
end
