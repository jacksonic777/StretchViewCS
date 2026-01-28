object frmVersion: TfrmVersion
  Left = 437
  Top = 530
  BorderStyle = bsDialog
  Caption = #12496#12540#12472#12519#12531#24773#22577
  ClientHeight = 317
  ClientWidth = 470
  Color = clBtnFace
  Font.Charset = SHIFTJIS_CHARSET
  Font.Color = clWindowText
  Font.Height = -12
  Font.Name = #65325#65331' '#65328#12468#12471#12483#12463
  Font.Style = []
  OldCreateOrder = True
  Position = poOwnerFormCenter
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 12
  object OKButton: TButton
    Left = 168
    Top = 264
    Width = 122
    Height = 29
    Caption = 'OK'
    Default = True
    ModalResult = 1
    TabOrder = 0
  end
  object Panel1: TPanel
    Left = 8
    Top = 8
    Width = 433
    Height = 241
    BevelInner = bvLowered
    TabOrder = 1
    object Version: TLabel
      Left = 232
      Top = 80
      Width = 77
      Height = 12
      Caption = #12496#12540#12472#12519#12531#30058#21495
      IsControl = True
    end
    object Label1: TLabel
      Left = 32
      Top = 24
      Width = 254
      Height = 21
      Caption = #12510#12523#12481#25313#22823#37857' StretchView'
      Font.Charset = SHIFTJIS_CHARSET
      Font.Color = clBlue
      Font.Height = -21
      Font.Name = #65325#65331' '#65328#12468#12471#12483#12463
      Font.Style = [fsBold]
      ParentFont = False
    end
    object Memo1: TMemo
      Left = 256
      Top = 96
      Width = 129
      Height = 25
      Lines.Strings = (
        'Memo1')
      TabOrder = 0
    end
    object Edit1: TEdit
      Left = 24
      Top = 128
      Width = 193
      Height = 17
      BevelEdges = [beTop, beRight, beBottom]
      BevelInner = bvNone
      BorderStyle = bsNone
      Color = clBtnFace
      Font.Charset = SHIFTJIS_CHARSET
      Font.Color = clBlue
      Font.Height = -12
      Font.Name = #65325#65331' '#65328#12468#12471#12483#12463
      Font.Style = []
      ParentFont = False
      TabOrder = 1
      Text = 'http://f29.aaa.livedoor.jp/~morg/wiki/'
    end
    object Memo2: TMemo
      Left = 72
      Top = 168
      Width = 281
      Height = 49
      Lines.Strings = (
        'Memo2')
      TabOrder = 2
    end
  end
end
