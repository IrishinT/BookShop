namespace BookShopLibrary
{
    /// <summary>
    /// Типы ошибок в поставке
    /// </summary>
    public enum DeliveryErrorType
    {
        None,           // Идеальная книга (всегда для заказов)
        Typo,           // Опечатка
        Plagiarism      // Плагиат
    }

    /// <summary>
    /// Результаты обслуживания покупателя
    /// </summary>
    public enum CustomerServiceResult
    {
        Success,        // Успешная продажа
        PriceTooHigh,   // Наценка больше 15%
        WrongBook,      // Предложена не та книга или не тот жанр
        NoCustomers     // Очередь пуста
    }

    /// <summary>
    /// Состояния игрового процесса
    /// </summary>
    public enum GameState
    {
        Playing,
        Won,
        Lost
    }
}