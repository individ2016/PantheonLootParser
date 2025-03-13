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
			.MinimumLevel.Error() // Минимальный уровень логирования
			.WriteTo.File(
				path: "logs/log-.txt", // Путь к файлу логов
				rollingInterval: RollingInterval.Day, // Ежедневные файлы
				retainedFileCountLimit: 2, // максимум 2 файла с логом
				outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}" // Формат записи
			)
			.Enrich.WithExceptionDetails() // Подробности об исключениях
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
				// Логирование необработанных исключений
				Log.Fatal(ex, "Необработанное исключение в приложении");
			} finally
			{
				// Завершение работы Serilog
				Log.CloseAndFlush();
			}
		}

		static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			Log.Fatal(e.Exception, "Необработанное исключение в UI-потоке");
			MessageBox.Show($"Произошла ошибка: {e.Exception.Message}" + "\r\n" + e.Exception.StackTrace, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		static void ExceptionHandler(object sender, UnhandledExceptionEventArgs args)
		{
			var exception = args.ExceptionObject as Exception;
			if(exception != null)
			{
				Log.Fatal(exception, "Необработанное исключение в фоновом потоке");
			} else
			{
				Log.Fatal("Необработанное исключение в фоновом потоке (неизвестный тип)");
			}

			if(args.IsTerminating)
			{
				MessageBox.Show("Произошла критическая ошибка. Приложение будет закрыто.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}