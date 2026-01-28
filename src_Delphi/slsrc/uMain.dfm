object frmMain: TfrmMain
  Left = 460
  Top = 222
  BorderStyle = bsNone
  Caption = 'Magnifier & Flipper'
  ClientHeight = 300
  ClientWidth = 300
  Color = clBtnFace
  Font.Charset = SHIFTJIS_CHARSET
  Font.Color = clWindowText
  Font.Height = -12
  Font.Name = 'FixedSys'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  OnDestroy = FormDestroy
  PixelsPerInch = 96
  TextHeight = 18
  object MouseTimer: TTimer
    Enabled = False
    Interval = 100
    OnTimer = MouseTimerTimer
    Left = 8
    Top = 8
  end
  object popupMenu: TPopupMenuEx
    Left = 80
    Top = 8
    object menuChangeScale: TMenuItem
      Caption = 'change &scale'
      object menuChangeScaleUp: TMenuItem
        Caption = '&+1'
        OnClick = menuChangeScaleUpClick
      end
      object menuChangeScaleDown: TMenuItem
        Caption = '&-1'
        OnClick = menuChangeScaleDownClick
      end
      object N1: TMenuItem
        Caption = '-'
      end
      object menuChangeScaleInput: TMenuItem
        Caption = 'in&put scale...'
        OnClick = menuChangeScaleInputClick
      end
    end
    object N2: TMenuItem
      Caption = '-'
    end
    object menuFilp: TMenuItem
      Caption = '&filp'
      object menuLeftSideRight: TMenuItem
        Caption = '&horizontal'
        OnClick = menuLeftSideRightClick
      end
      object menuUpSideDown: TMenuItem
        Caption = '&vertical'
        OnClick = menuUpSideDownClick
      end
    end
    object N3: TMenuItem
      Caption = '-'
    end
    object menuSetting: TMenuItem
      Caption = 'se&tting'
      object menuSettingCrossVisible: TMenuItem
        Caption = 'cr&oss cursor'
        OnClick = menuSettingCrossVisibleClick
      end
      object menuSettingInfo: TMenuItem
        Caption = 'info&mation'
        object menuSettingInfoDecimal: TMenuItem
          Caption = '&decimal'
          RadioItem = True
          OnClick = menuSettingInfoDecimalClick
        end
        object menuSettingInfoHex: TMenuItem
          Caption = '&hex'
          RadioItem = True
          OnClick = menuSettingInfoHexClick
        end
        object N6: TMenuItem
          Caption = '-'
        end
        object menuSettingInfoVisible: TMenuItem
          Caption = '&visible'
          OnClick = menuSettingInfoClick
        end
      end
      object N5: TMenuItem
        Caption = '-'
      end
      object menuSettingRate: TMenuItem
        Caption = 'sampling &rate...'
        OnClick = menuSettingRateClick
      end
    end
    object menuVersion: TMenuItem
      Caption = 'version &info...'
      OnClick = menuVersionClick
    end
    object N4: TMenuItem
      Caption = '-'
    end
    object menuExit: TMenuItem
      Caption = 'e&xit'
      OnClick = menuExitClick
    end
  end
end
