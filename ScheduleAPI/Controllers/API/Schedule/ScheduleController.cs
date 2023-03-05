﻿using Microsoft.AspNetCore.Mvc;
using ScheduleAPI.Controllers.Data.Getter.Schedule;
using ScheduleAPI.Controllers.Other.General;
using ScheduleAPI.Models.Elements;
using ScheduleAPI.Models.Result.Schedule;

namespace ScheduleAPI.Controllers.API.Schedule
{
    /// <summary>
    /// Класс-контроллер для получения данных о расписании.
    /// <br/>
    /// Ранее он был разделен на 4 класса: для получения данных через БД/Ассеты и на День/Неделю.
    /// Теперь получение через БД удалено, а День/Неделя объединены в один класс.
    /// </summary>
    [Route("~/api/[controller]")]
    public class ScheduleController : Controller
    {
        #region Область: Поля.

        /// <summary>
        /// Поле, содержащее объект, нужный для получения расписания из ассетов.
        /// </summary>
        private readonly AssetGetter getter;
        #endregion

        #region Область: Свойства.

        /// <summary>
        /// Свойство с автоматически инициализированным логгером.
        /// </summary>
        public static ILogger<ScheduleController>? Logger { get; private set; } = null;
        #endregion

        #region Область: Конструкторы.

        /// <summary>
        /// Конструктор класса. Инициализирует значения полей и свойств.
        /// </summary>
        /// <param name="environment">Сведения об окружении, в котором работает приложение.
        /// Необходимо для корректной работы с файлами-ассетами.</param>
        /// <param name="logger">Автоматически инициализированный сервис логгера для работы.</param>
        public ScheduleController(IHostEnvironment environment, ILogger<ScheduleController> logger)
        {
            getter = new(environment);
            Logger = logger;
        }
        #endregion

        #region Область: Обработчики API.

        /// <summary>
        /// Метод, реализующий Get-запрос на получение расписания.
        /// </summary>
        /// <param name="dayIndex">Индекс дня для получения расписания.
        /// <br/>
        /// Значение по умолчанию: 0.</param>
        /// <param name="groupName">Название группы для получения расписания.
        /// <br/>
        /// Значение по умолчанию: 19П-3.</param>
        /// <returns>Json-объект, содержащий расписание для указанной группы в указанный день.</returns>
        [HttpGet]
        [Route("~/api/[controller]/day")]
        public JsonResult GetDaySchedule(int dayIndex = 0, string groupName = "19П-3")
        {
            groupName = groupName.RemoveStringChars();
            dayIndex = dayIndex.CheckDayIndexFromOverflow();

            DaySchedule schedule = getter.GetDaySchedule(dayIndex, groupName);

            return new JsonResult(schedule, JsonSettingsModel.JsonOptions);
        }

        /// <summary>
        /// Метод, реализующий Get-запрос на получение расписания.
        /// </summary>
        /// <param name="groupName">Название группы для получения расписания.
        /// <br/>
        /// Значение по умолчанию: 19П-3.</param>
        /// <returns>Json-объект, содержащий расписание для указанной группы в указанный день.</returns>
        [HttpGet]
        [Route("~/api/[controller]/week")]
        public JsonResult GetWeekSchedule(string groupName = "19П-3")
        {
            groupName = groupName.RemoveStringChars();

            WeekSchedule schedule = getter.GetWeekSchedule(groupName);

            return new JsonResult(schedule, JsonSettingsModel.JsonOptions);
        }
        #endregion
    }
}
