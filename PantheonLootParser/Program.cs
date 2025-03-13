using Serilog;
using Serilog.Exceptions;

namespace PantheonLootParser
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Error() // ����������� ������� �����������
			.WriteTo.File(
				path: "logs/log-.txt", // ���� � ����� �����
				rollingInterval: RollingInterval.Day, // ���������� �����
				retainedFileCountLimit: 2, // �������� 2 ����� � �����
				outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}" // ������ ������
			)
			.Enrich.WithExceptionDetails() // ����������� �� �����������
			.CreateLogger();

			try
			{
				// To customize application configuration such as set high DPI settings or default font,
				// see https://aka.ms/applicationconfiguration.
				ApplicationConfiguration.Initialize();
				Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
				AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(ExceptionHandler);
				Application.ThreadException += Application_ThreadException;

				Application.Run(new MainForm());
			} catch(Exception ex)
			{
				// ����������� �������������� ����������
				Log.Fatal(ex, "�������������� ���������� � ����������");
			} finally
			{
				// ���������� ������ Serilog
				Log.CloseAndFlush();
			}
		}

		static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			Log.Fatal(e.Exception, "�������������� ���������� � UI-������");
			MessageBox.Show($"��������� ������: {e.Exception.Message}" + "\r\n" + e.Exception.StackTrace, "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		static void ExceptionHandler(object sender, UnhandledExceptionEventArgs args)
		{
			var exception = args.ExceptionObject as Exception;
			if(exception != null)
			{
				Log.Fatal(exception, "�������������� ���������� � ������� ������");
			} else
			{
				Log.Fatal("�������������� ���������� � ������� ������ (����������� ���)");
			}

			if(args.IsTerminating)
			{
				MessageBox.Show("��������� ����������� ������. ���������� ����� �������.", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}