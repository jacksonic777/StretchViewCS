using System;
using System.Windows.Forms;
using System.Threading;
using StretchViewCS.Forms;
using StretchViewCS.Utils;

namespace StretchViewCS
{
    static class Program
    {
        private const string AppTitle = "StretchViewCS";
        private static Mutex? mutex;

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Mutexによる二重起動防止（現在はコメントアウト）
            /*
            bool createdNew;
            mutex = new Mutex(true, AppTitle, out createdNew);
            if (!createdNew)
            {
                Application.Exit();
                return;
            }
            */

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 初回実行時の処理
            if (IniManager.Instance.RunCount >= 0)
            {
                // ライセンスキー確認（現在はコメントアウト）
                /*
                if (IniManager.Instance.LicenseKey != "BWEXPERP")
                {
                    string input = Microsoft.VisualBasic.Interaction.InputBox(
                        AppTitle + " ライセンスキー入力",
                        "本ソフトウェアをご利用いただきありがとうございます。\r\n" +
                        "配布サイト(Vector等)で配布の際のライセンスキーを入力してください。\r\n" +
                        "新規購入はVectorサイト等で可能です。",
                        "");

                    if (string.IsNullOrEmpty(input))
                    {
                        Application.Exit();
                        return;
                    }

                    if (input != "BWEXPERP")
                    {
                        MessageBox.Show("正しくありませんでした。\r\n終了します。", AppTitle);
                        Application.Exit();
                        return;
                    }
                }
                else
                {
                    IniManager.Instance.RunCount = 10;
                }
                */
            }

            // フォームの作成
            Application.Run(new frmCap());
        }
    }
}
