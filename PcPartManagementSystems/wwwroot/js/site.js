// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Hide nav-group-items initially
$(document).ready(function () {
    $('.nav-group-items').hide();

    // Toggle nav-group-items visibility on click
    $('.nav-group-toggle').click(function (event) {
        event.preventDefault(); // Prevent default link behavior
        $(this).next('.nav-group-items').slideToggle();
    });

    // Toggle sidebar visibility on hamburger_menu click
    $('.hamburger_menu').click(function () {
        $('.sidebar').toggleClass('showsidebar');
    });

    // on click hide modal
    $('.back-button').click(function () {
        $('.Modal_error').hide()
    });
});



// Function to display loading indicator for 10 seconds
function BtnClick() {
    var dvLoading = document.getElementById('dvLoading');

    if (dvLoading != null) {
        dvLoading.style.display = 'block'; // Display the loading indicator
    }

    setTimeout(function () {
        if (dvLoading != null) {
            dvLoading.style.display = 'none'; // Hide the loading indicator after 10 seconds
        }

        // Proceed with form submission or other actions
        // Example: document.forms['myForm'].submit();
    }, 10000); // 10000 milliseconds = 10 seconds

    return true; // Return true to allow the form submission to proceed
}

// Function to format date without time
function dateWithoutTime(date) {
    var dd = String(date.getDate()).padStart(2, '0');
    var mm = String(date.getMonth() + 1).padStart(2, '0');
    var yyyy = date.getFullYear();
    var formattedDateWithoutTime = mm + '/' + dd + '/' + yyyy;
    return formattedDateWithoutTime;
}

// Function to format date with time
function dateTimeFormat(date) {
    var dd = String(date.getDate()).padStart(2, '0');
    var mm = String(date.getMonth() + 1).padStart(2, '0');
    var yyyy = date.getFullYear();
    var hours = String(date.getHours()).padStart(2, '0');
    var minutes = String(date.getMinutes()).padStart(2, '0');
    var seconds = String(date.getSeconds()).padStart(2, '0');

    var formattedDateWithTime = mm + '/' + dd + '/' + yyyy + ' ' + hours + ':' + minutes + ':' + seconds;
    return formattedDateWithTime;
}

// Function to format currency amount
function formatCurrency(amount) {
    amount = parseFloat(amount);
    var formattedAmount = amount.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
    return formattedAmount;
}

////// Select2

// Function to populate a Select2 dropdown from a URL
function setSelect2(url, selector, selectedValue) {
    // Make an AJAX request to fetch data from the specified URL
    $.ajax({
        url: url,
        method: 'GET',
        dataType: 'json', // Assuming data is returned as JSON
        success: function (data) {
            // Clear existing options in the dropdown
            $(selector).empty();

            // Populate dropdown with fetched data
            $.each(data, function (index, item) {
                // Create an option element for each item in the data
                var option = '<option value="' + item.id + '">' + item.text + '</option>';

                // Check if selectedValue matches item.id and mark as selected
                if (selectedValue != null && item.id === selectedValue) {
                    option = '<option value="' + item.id + '" selected="selected">' + item.text + '</option>';
                }

                // Append the option to the dropdown
                $(selector).append(option);
            });

            // Trigger change event to update Select2 UI
            $(selector).trigger('change');
        },
        error: function (xhr, status, error) {
            console.error('Error fetching data:', error);
            // Handle error scenario as needed
        }
    });
}


function setSelect2(url, selector) {
    // Make an AJAX request to fetch data from the specified URL
    $.ajax({
        url: url,                // URL to fetch data from
        method: 'GET',           // HTTP method (GET)
        dataType: 'json',        // Data type expected from the server (JSON)
        success: function (data) {
            // Successful AJAX request handler

            // Clear existing options in the dropdown
            $(selector).empty();

            // Populate dropdown with fetched data
            $.each(data, function (index, item) {
                // Append an <option> element for each item in the data
                $(selector).append('<option value="' + item.id + '">' + item.text + '</option>');
            });

            // Trigger change event to update Select2 UI
            $(selector).trigger('change');
        },
        error: function (xhr, status, error) {
            // Error handler for AJAX request
            console.error('Error fetching data:', error);

            // Handle error scenario as needed
            // For example, display an error message or retry the request
        }
    });
}
