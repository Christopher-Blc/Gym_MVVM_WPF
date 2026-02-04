using Centro_Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Centro_ViewModel.Infrastructure
{
    /// <summary>
    /// ViewModel encargado de la gestion de socios.
    /// Proporciona la lista de socios, el socio seleccionado, los campos del formulario
    /// y los comandos para crear, editar y eliminar socios.
    /// </summary>
    public class SociosViewModel : BaseViewModel
    {

        /// <summary>
        /// Lista de socios.
        /// </summary>
        private List<Socios> listaSocios;
        public List<Socios> ListaSocios
        {
            get => listaSocios;
            set
            {
                listaSocios = value;
                OnPropertyChanged(nameof(ListaSocios));
            }
        }

        /// <summary>
        /// Socio seleccionado en la vista.
        /// Al establecerlo se cargan los campos del formulario.
        /// </summary>
        private Socios socioSeleccionado;
        public Socios SocioSeleccionado
        {
            get => socioSeleccionado;
            set
            {
                socioSeleccionado = value;
                OnPropertyChanged(nameof(SocioSeleccionado));

                // Al seleccionar fila, cargamos los TextBox
                if (socioSeleccionado != null)
                {
                    Nombre = socioSeleccionado.Nombre;
                    Email = socioSeleccionado.Email;
                    IsActive = socioSeleccionado.Activo;
                }
                else
                {
                    Nombre = "";
                    Email = "";
                    IsActive = true;
                }
            }
        }


        /// <summary>
        /// Valor del campo Nombre del formulario.
        /// </summary>
        private string nombre;
        public string Nombre
        {
            get => nombre;
            set { nombre = value; OnPropertyChanged(nameof(Nombre)); }
        }

        /// <summary>
        /// Valor del campo Email del formulario.
        /// </summary>
        private string email;
        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(nameof(Email)); }
        }

        /// <summary>
        /// Estado activo del socio.
        /// </summary>
        private bool isActive;
        public bool IsActive
        {
            get => isActive;
            set { isActive = value; OnPropertyChanged(nameof(IsActive)); }
        }


        // Commands

        /// <summary>
        /// Comando para crear un nuevo socio.
        /// </summary>
        public ICommand CrearSocioCommand { get; }

        /// <summary>
        /// Comando para editar un socio.
        /// </summary>
        public ICommand EditarSocioCommand { get; }

        /// <summary>
        /// Comando para eliminar un socio.
        /// </summary>
        public ICommand EliminarSocioCommand { get; }

        /// <summary>
        /// Constructor. Inicializa los comandos y recarga los datos.
        /// </summary>
        public SociosViewModel()
        {
            CrearSocioCommand = new RelayCommand(Crear);
            EditarSocioCommand = new RelayCommand(Editar);
            EliminarSocioCommand = new RelayCommand(Eliminar);
            //Seteamos el isactive al false para evitar comprobaciones innecesarias
            IsActive = true;
            Recargar();
        }

        /// <summary>
        /// Carga los socios desde la base de datos.
        /// </summary>
        /// <returns>void</returns>
        private void Recargar()
        {
            using (var contexto = new CentroDeportivoEntities())
            {
                ListaSocios = contexto.Socios.ToList();
            }
        }

        /// <summary>
        /// Valida los campos del formulario y crea un nuevo socio.
        /// Al finalizar recarga la lista y limpia el formulario.
        /// </summary>
        /// <returns>void</returns>
        private void Crear()
        {
            if (!Validaciones.SocioFormularioValido(Nombre, Email, out var error))
            {
                MessageBox.Show(error);
                return;
            }

            using (var contexto = new CentroDeportivoEntities())
            {
                var nuevo = new Socios
                {
                    Nombre = Validaciones.LimpiarTexto(Nombre),
                    Email = Validaciones.LimpiarTexto(Email),
                    Activo = IsActive
                };

                contexto.Socios.Add(nuevo);
                contexto.SaveChanges();
            }

            Recargar();
            LimpiarFormulario();
        }


        /// <summary>
        /// Modifica el socio seleccionado con los valores del formulario.
        /// Valida los datos y persiste los cambios en la base de datos.
        /// </summary>
        /// <returns>void</returns>
        private void Editar()
        {
            if (SocioSeleccionado == null)
            {
                MessageBox.Show("Ningun socio seleccionada");
                return;
            }

            if (!Validaciones.SocioFormularioValido(Nombre, Email, out var error))
            {
                MessageBox.Show(error);
                return;
            }

            using (var contexto = new CentroDeportivoEntities())
            {
                var socioActual = contexto.Socios.Find(SocioSeleccionado.Id);
                if (socioActual == null) return;

                socioActual.Nombre = Validaciones.LimpiarTexto(Nombre);
                socioActual.Email = Validaciones.LimpiarTexto(Email);
                socioActual.Activo = IsActive;
                contexto.SaveChanges();
            }

            Recargar();
            LimpiarFormulario();
        }

        /// <summary>
        /// Elimina el socio seleccionado de la base de datos.
        /// </summary>
        /// <returns>void</returns>
        private void Eliminar()
        {
            if (SocioSeleccionado == null)
            {
                MessageBox.Show("Ningun socio seleccionada");
                return;
            }

            using (var contexto = new CentroDeportivoEntities())
            {
                var socioActual = contexto.Socios.Find(SocioSeleccionado.Id);
                if (socioActual == null) return;

                contexto.Socios.Remove(socioActual);
                contexto.SaveChanges();
            }

            Recargar();
            LimpiarFormulario();
        }

        /// <summary>
        /// Limpia los campos del formulario y deselecciona el socio.
        /// </summary>
        /// <returns>void</returns>
        private void LimpiarFormulario()
        {
            Nombre = "";
            Email = "";
            SocioSeleccionado = null;
            IsActive = true;
        }
    }
}
