public List<Book> MyContain(string input)
{
    List<Book> books = new List<Book>();
    bool isMatch = false;

    foreach (var book in DataBase.Books)
    {
        if (book.Title.Length < input.Length)
            continue;

        isMatch = false;

        for (int i = 0; i <= book.Title.Length - input.Length; i++)
        {
            int j;
            for (j = 0; j < input.Length; j++)
            {
                if (input[j] != book.Title[i + j]) // cause j always starts with 0 we can have (J + I)
                {
                    break;
                }
            }

            if (j == input.Length)
            {
                isMatch = true;
                break;
            }
        }

        if (isMatch)
        {
            books.Add(book);
        }
    }

    return books;
}