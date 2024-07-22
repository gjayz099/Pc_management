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

    // on click hide modal
    $('.back-button').click(function () {
        $('.Modal_error').hide()
    });

    $('#ErrorShowModal').modal('show');

    $('#ErrorShowModalHide').click(function () {
        $('#ErrorShowModal').modal('hide');
    })


    $('#SuccessShowModal').modal('show');

    $('#SuccessShowModal').click(function () {
        $('#SuccessShowModal').modal('hide');
    })


    $('#dropdown-toggle').click(function (e) {
        e.preventDefault(); // Prevent default anchor behavior

        var currentState = $(this).attr('aria-expanded');

        // Toggle aria-expanded attribute
        if (currentState === 'true') {
            $(this).attr('aria-expanded', 'false');
        } else {
            $(this).attr('aria-expanded', 'true');
        }

        // Optionally, toggle the visibility of the dropdown menu
        var dropdownMenu = $(this).next('.dropdown-menu');
        dropdownMenu.toggle(); // Toggle visibility of the dropdown menu
    });
  



});




// Function to update total price display
function updateTotalPrice() {
    var totalPrice = 0;
    $('.totalPrice').each(function () {
        var price = parseFloat($(this).val()) || 0; // Parse price value to float, default to 0 if NaN
        totalPrice += price;
    });
    $('#totalPriceDisplay').text(formatCurrency(totalPrice)); // Update total price display
    $('#totalPriceDisplayHide').val(formatCurrency(totalPrice)); // Update total price display
}


// Function to display loading indicator for 10 seconds
function BtnClick() {
    var dvLoading = document.getElementById('dvLoading');

    if (dvLoading != null) {
        dvLoading.style.display = 'block'; // Display the loading indicator
    }

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

function FileImg(img) {
    return '<img src="/img/' + img + '" alt="Image" style="width: 50px; height: 50px;"/>';
}



function GetDataInMoneyAjax(url, value) {
    $.ajax({
        url: url,
        dataSrc: '',
        success: function (data) {
            $(value).text(formatCurrency(data));
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error('Error fetching data:', textStatus, errorThrown);
            Swal.fire({ icon: "error", title: errorThrown });
        },
    })
}

function GetDataInCountjax(url, value) {
    $.ajax({
        url: url,
        dataSrc: '',
        success: function (data) {
            $(value).text(data);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error('Error fetching data:', textStatus, errorThrown);
            Swal.fire({ icon: "error", title: errorThrown });
        },
    })
}

// Function to populate a Select2 dropdown from a URL
function setSelect2ID(url, selector, selectedValue) {
    // Make an AJAX request to fetch data from the specified URL
    $.ajax({
        url: url,
        method: 'GET',
        dataType: 'json', // Assuming data is returned as JSON
        success: function (data) {
            // Clear existing options in the dropdown
            $(selector).empty();


            // Add a placeholder option if no selected value is provided
            if (!selectedValue) {
                var placeholderOption = '<option  class="form-control" value="" disabled selected> ---Select--- </option>';
                $(selector).append(placeholderOption);
            }
            // Populate dropdown with fetched data
            $.each(data, function (index, item) {
                // Create an option element for each item in the data
                var option = '<option  class="form-control" value="' + item.id + '">' + item.text + '</option>';

                // Check if selectedValue matches item.id and mark as selected
                if (selectedValue != null && item.id === selectedValue) {
                    option = '<option  class="form-control"  value="' + item.id + '" selected="selected">' + item.text + '</option>';
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


// Function to fetch and populate Select2 dropdown
function setSelect2ID(url, selector, selectedValue, selectme) {
    $.ajax({
        url: url,
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            $(selector).empty();

            // Add a placeholder option
            var placeholderOption = '<option value="" disabled selected> ---Select--- </option>';
            $(selector).append(placeholderOption);

            // Populate dropdown with fetched data
            $.each(data, function (index, item) {
                var option = '<option value="' + item.id + '">' + item.text + '</option>';
                $(selector).append(option);
            });

            // Initialize Select2 dropdown
            $(selector).select2();

            // Event listener for dropdown change
            $(selector).on('change', function () {
                var selectedId = $(this).val();
                var selectedItem = data.find(x => x.id === selectedId);

                if (selectedItem) {
                    if (selectme) {
                        $(selectme).val(selectedItem.value1); // Assuming 'value1' contains the price
                   
                    }
                }
            });

            // Set selected value and trigger change event
            if (selectedValue !== null && selectme) {
                $(selector).val(selectedValue);
                $(selector).trigger('change');
            }
        },
        error: function (xhr, status, error) {
            console.error('Error fetching data:', error);
        }
    });
}







function setSelect2Name(url, selector, selectedValue) {
    $.ajax({
        url: url,
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            console.log('Data fetched:', data); // Log fetched data
            $(selector).empty();
            if (!selectedValue) {
                var placeholderOption = '<option value="" disabled selected> ---Select--- </option>';
                $(selector).append(placeholderOption);
            }
            // Populate dropdown with fetched data
            $.each(data, function (index, item) {
                // Create an option element for each item in the data
                var option = '<option value="' + item.id + '">' + item.text + '</option>';

                // Check if selectedValue matches item.id and mark as selected
                if (selectedValue != null && item.text === selectedValue) {
                    option = '<option value="' + item.id + '" selected="selected">' + item.text + '</option>';
                }

                // Append the option to the dropdown
                $(selector).append(option);
            });

            // Re-initialize Select2 to reflect the changes
            $(selector).trigger('change');
        },
        error: function (xhr, status, error) {
            console.error('Error fetching data:', error);
        }
    });
}

// Function to set Select2 options after fetching data via AJAX

function setSelect2WithSearch(url, selector) {
    $.ajax({
        url: url,
        method: 'GET',
        dataType: 'json',
        data: function (params) {
            return {
                q: params.term, // search term
                page: params.page
            };
        },
        success: function (data) {
            $(selector).empty(); // Clear existing options in the dropdown

            // Add the placeholder option
            $(selector).append('<option value="">--Select--</option>');

            // Populate dropdown with fetched data
            $.each(data, function (index, item) {
                $(selector).append('<option value="' + item.id + '">' + item.text + '</option>');
            });

            // Trigger change event to update Select2
            $(selector).trigger('change.select2');
        },
        error: function (xhr, status, error) {
            console.error('Error fetching data:', error);
            // Handle error scenario as needed
        }
    });
}



function setSelect2WithoutSearch(url, selector) {
    $.ajax({
        url: url,
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            $(selector).empty(); // Clear existing options in the dropdown

            // Add the placeholder option
            $(selector).append('<option value="">--Select--</option>');

            // Populate dropdown with fetched data
            $.each(data, function (index, item) {
                $(selector).append('<option value="' + item.id + '">' + item.text + '</option>');
            });

            // Trigger change event to update Select2
            $(selector).trigger('change.select2');
        },
        error: function (xhr, status, error) {
            console.error('Error fetching data:', error);
            // Handle error scenario as needed
        }
    });
}

