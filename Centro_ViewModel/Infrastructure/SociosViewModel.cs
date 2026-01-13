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
    public class SociosViewModel : BaseViewModel
    {

        //Lista de socios y obtener el socio seleccionado del datagrid
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


        //Campos del formulario
        private string nombre;
        public string Nombre
        {
            get => nombre;
            set { nombre = value; OnPropertyChanged(nameof(Nombre)); }
        }

        private string email;
        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(nameof(Email)); }
        }

        private bool isActive;
        public bool IsActive
        {
            get => isActive;
            set { isActive = value; OnPropertyChanged(nameof(IsActive)); }
        }


        //Commands
        public ICommand CrearSocioCommand { get; }
        public ICommand EditarSocioCommand { get; }
        public ICommand EliminarSocioCommand { get; }

        public SociosViewModel()
        {
            CrearSocioCommand = new RelayCommand(Crear);
            EditarSocioCommand = new RelayCommand(Editar);
            EliminarSocioCommand = new RelayCommand(Eliminar);
            //Seteamos el isactive al false para evitar comprobaciones innecesarias
            IsActive = true;
            Recargar();
        }

        private void Recargar()
        {
            using (var contexto = new CentroDeportivoEntities())
            {
                ListaSocios = contexto.Socios.ToList();
            }
        }

        private void Crear()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                MessageBox.Show("El nombre no puede estar vacio");
                return;
            }
            if (string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("El Email no puede estar vacio");
                return;
            }
            if (!(Email.Contains("@")))
            {
                MessageBox.Show("El Email no tiene un formato correcto");
                return;
            }

            //crear el socio con los datos recibidos 
            using (var contexto = new CentroDeportivoEntities())
            {
                var nuevo = new Socios
                {
                    Nombre = Nombre.Trim(),
                    Email = Email.Trim(),
                    Activo = IsActive
                };

                contexto.Socios.Add(nuevo);
                contexto.SaveChanges();
            }

            Recargar();
            LimpiarFormulario();
        }



        //Metodo para modificar
        private void Editar()
        {
            if (SocioSeleccionado == null)
            {
                MessageBox.Show("Ningun socio seleccionada");
                return;
            }
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                MessageBox.Show("El nombre no puede estar vacio");
                return;
            }
            if (string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("El Email no puede estar vacio");
                return;
            }
            if (!(Email.Contains("@")))
            {
                MessageBox.Show("El Email no tiene un formato correcto");
                return;
            }

            using (var contexto = new CentroDeportivoEntities())
            {
                var socioActual = contexto.Socios.Find(SocioSeleccionado.Id);
                if (socioActual == null) return;

                socioActual.Nombre = Nombre.Trim();
                socioActual.Email = Email.Trim();
                socioActual.Activo = IsActive;
                contexto.SaveChanges();
            }

            Recargar();
            LimpiarFormulario();
        }

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

        private void LimpiarFormulario()
        {
            Nombre = "";
            Email = "";
            SocioSeleccionado = null;
            IsActive = true;
        }
    }
}
