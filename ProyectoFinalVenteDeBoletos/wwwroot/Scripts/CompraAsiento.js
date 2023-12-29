const container = document.querySelector('.container');
const seats = document.querySelectorAll('.row .seat:not(.occupied');
const count = document.getElementById('count');
const total = document.getElementById('total');
const movieSelect = document.getElementById('movie');

populateUI();
let ticketPrice = +movieSelect.value;

// Save selected movie index and price
function setMovieData(movieIndex, moviePrice) {
  localStorage.setItem('selectedMovieIndex', movieIndex);
  localStorage.setItem('selectedMoviePrice', moviePrice);
}

// update total and count
function updateSelectedCount() {
  const selectedSeats = document.querySelectorAll('.row .seat.selected');

  const seatsIndex = [...selectedSeats].map((seat) => [...seats].indexOf(seat));

  localStorage.setItem('selectedSeats', JSON.stringify(seatsIndex));

  //copy selected seats into arr
  // map through array
  //return new array of indexes

  const selectedSeatsCount = selectedSeats.length;

  count.innerText = selectedSeatsCount;
  total.innerText = selectedSeatsCount * ticketPrice;
}

// get data from localstorage and populate ui
function populateUI() {
  const selectedSeats = JSON.parse(localStorage.getItem('selectedSeats'));
  if (selectedSeats !== null && selectedSeats.length > 0) {
    seats.forEach((seat, index) => {
      if (selectedSeats.indexOf(index) > -1) {
        seat.classList.add('selected');
      }
    });
  }

  const selectedMovieIndex = localStorage.getItem('selectedMovieIndex');

  if (selectedMovieIndex !== null) {
    movieSelect.selectedIndex = selectedMovieIndex;
  }
}

// Movie select event
movieSelect.addEventListener('change', handleMovieChange);
function handleMovieChange(e) {
    ticketPrice = +e.target.value;
    setMovieData(e.target.selectedIndex, e.target.value);
    updateSelectedCount();
}


// Seat click event
//document.querySelectorAll('.row .seat:not(.occupied)').forEach((seat) => {
//    seat.addEventListener('click', (e) => {
//        if (!e.target.classList.contains('occupied')) {
//            e.target.classList.toggle('selected');
//            updateSelectedCount();
//        }
//    });
//});


// Seat click event Enlazada para enviar datos al ViewModel
document.querySelectorAll('.row .seat:not(.occupied)').forEach((seat) => {
    seat.addEventListener('click', (e) => {
        if (!e.target.classList.contains('occupied')) {
            e.target.classList.toggle('selected');

            // Aquí enviamos la información al servidor
            const seatNumber = e.target.innerText;
            const isSelected = e.target.classList.contains('selected');

            $.ajax({
                type: 'POST',
                url: 'HomeController/GuardarSeleccion',
                data: { seatNumber: seatNumber, isSelected: isSelected },
                success: function (data) {
                    console.log('Información enviada al servidor con éxito.');
                },
                error: function (error) {
                    console.error('Error al enviar la información al servidor.');
                }
            });

            updateSelectedCount();
        }
    });
});

// intial count and total
updateSelectedCount();