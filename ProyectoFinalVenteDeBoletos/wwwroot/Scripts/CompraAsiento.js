//Contenedor
const container = document.querySelector('.container');
//Asientos disponibles para seleccionar
const seats = document.querySelectorAll('.row .seat:not(.occupied');
//Cantidad de asientos seleccionados
const count = document.getElementById('count');
//Precio de la pelicula * asientos seleccionados
const total = document.getElementById('total');
//Pelicula
const movieSelect = document.getElementById('movie');

populateUI();
//precio de la pelicula
let ticketPrice = +movieSelect.value;

// almacenar index and precio de la pelicula seleccionada
function setMovieData(movieIndex, moviePrice) {
  localStorage.setItem('selectedMovieIndex', movieIndex);
  localStorage.setItem('selectedMoviePrice', moviePrice);
}

// actualizar total and count
function updateSelectedCount()
{
    //Asientos Seleccionados
    const selectedSeats = document.querySelectorAll('.row .seat.selected');
    //Obtener los Index de los asientos seleccionados
    const seatsIndex = [...selectedSeats].map((seat) => [...seats].indexOf(seat));
    localStorage.setItem('selectedSeats', JSON.stringify(seatsIndex));
    //regresar la cantidad de asientos seleccionados
    const selectedSeatsCount = selectedSeats.length;
    //Cambiar textos en HTML
    count.innerText = selectedSeatsCount;
    total.innerText = selectedSeatsCount * ticketPrice;
}

// get data from localstorage and populate ui
function populateUI() {
    //convertir asientos seleccionados en json
    const selectedSeats = JSON.parse(localStorage.getItem('selectedSeats'));
  //si hay asientos
    if (selectedSeats !== null && selectedSeats.length > 0) {
      //buscar la posicion del asiento
    seats.forEach((seat, index) => {
        if (selectedSeats.indexOf(index) > -1) {
            //Agregar asiento
            seat.classList.add('selected');
        }
    });
  }
  //Obtener index de la pelicula
  const selectedMovieIndex = localStorage.getItem('selectedMovieIndex');

  //si hay pelicula
    if (selectedMovieIndex !== null) {
      //asignar index
    movieSelect.selectedIndex = selectedMovieIndex;
  }
}

//??
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

//??
// Seat click event Enlazada para enviar datos al ViewModel
seats.forEach((seat) =>
{
    //Agregamos evento click a los asientos
    seat.addEventListener('click', (e) => {
        e.target.classList.toggle('selected');
        // Aqui enviamos la informacion al servidor
        const seatNumber = e.target.innerText;
        const isSelected = e.target.classList.contains('selected');
        // Peticion al servidor(esto lo haria el boton)
        $.ajax({
            //Tipo de peticion
            type: 'POST',
            //URL de peticion(accion en el controlador)
            url: 'HomeController/GuardarSeleccion',
            //Contenido de la peticion(parametros en el controlador)
            data: { seatNumber: seatNumber, isSelected: isSelected },
            //En caso de que la peticion sea exitosa
            success: function (data) {
                console.log('Informacion enviada al servidor con exito.');
            },
            //En Caso de que no
            error: function (error) {
                console.error('Error al enviar la información al servidor.');
            }
        });
        //Actualizar contador
        updateSelectedCount();
    });
});

// intial count and total
updateSelectedCount();