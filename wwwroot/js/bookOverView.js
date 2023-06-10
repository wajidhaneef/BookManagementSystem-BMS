
$(document).ready(function () {

    loadBooks();
    loadChapters();
    //loadChapterContent();
    console.log("run on the load page")
});
function loadBooks() {
    console.log("load books")
    let categoryId = $("#categoryDropdown").val();
    if (categoryId !== "") {
        $.ajax({
            url: '@Url.Action("GetBooksByCategory")',
            type: 'GET',
            data: { categoryId: categoryId },
            success: function (result) {
                $("#books").html(result);
            },
            error: function () {
                alert("An error occurred while loading chapters.");
            }
        });
    }
    else {
        $("#books").html("<option value=''>Select a Category first</option>");
    }
}



function loadChapters() {
    console.log("load chapters")
    let bookId = $("#selectedBook").val();
    if (bookId !== "") {
        $.ajax({
            url: '@Url.Action("GetChaptersByBook")',
            type: 'GET',
            data: { bookId: bookId },
            success: function (result) {
                $("#chapters").html(result);
                $("#chapterContent").html("<p>Select a chapter to view its content.</p>");
            },
            error: function () {
                alert("An error occurred while loading chapters.");
            }
        });
    }
    else {
        $("#chapterDropdown").html("<option value=''>Select a book first</option>");
        $("#chapterContent").html("<p>Select a chapter to view its content.</p>");
    }
}

function loadChapterContent() {
    console.log("load chapters")
    let chapterId = $("#selectedChapter").val();
    if (chapterId !== "") {
        $.ajax({
            url: '@Url.Action("GetChapterContent")',
            type: 'GET',
            data: { chapterId: chapterId },
            success: function (result) {
                $("#chapterContent").html("<p>" + result + "</p>");
            },
            error: function () {
                alert("An error occurred while loading chapter content.");
            }
        });
    }
    else {
        $("#chapterContent").html("<p>Select a chapter to view its content.</p>");
    }
}