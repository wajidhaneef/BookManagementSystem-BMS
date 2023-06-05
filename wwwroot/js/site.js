// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('#selectedCategory').change(function () {
    var categoryId = $("#selectedCategory").val();

    if (categoryId !== '') {
        $.ajax({
            type: 'GET',
            url: '@Url.Action("GetBooksByCategory", "Book")',
            data: { categoryId: categoryId },
            success: function (result) {
                $('#booksDropdown').html(result);
            }
        });
    } else {
        $('#booksDropdown').empty();
        $('#chaptersDropdown').empty();
        $('#chapterContent').empty();
    }
});

$('#booksDropdown').on('change', '#selectedBook', function () {
    var bookId = $(this).val();

    if (bookId !== '') {
        $.ajax({
            type: 'GET',
            url: '@Url.Action("GetChaptersByBook", "Book")',
            data: { bookId: bookId },
            success: function (result) {
                $('#chaptersDropdown').html(result);
            }
        });
    } else {
        $('#chaptersDropdown').empty();
        $('#chapterContent').empty();
    }
});

$('#chaptersDropdown').on('change', '#selectedChapter', function () {
    var chapterId = $(this).val();

    if (chapterId !== '') {
        $.ajax({
            type: 'GET',
            url: '@Url.Action("GetChapterContent", "Book")',
            data: { chapterId: chapterId },
            success: function (result) {
                $('#chapterContent').html('<p>' + result + '</p>');
            }
        });
    } else {
        $('#chapterContent').empty();
    }
});

