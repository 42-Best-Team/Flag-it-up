using System;
using System.Linq;
using System.Windows.Forms;

public abstract class UserUI : Form
{
    protected DataGridView dgvUsers;
    protected TextBox txtName;
    protected Button btnAdd;
    protected Button btnEdit;
    protected Button btnDelete;

    protected Repository<Player> userRepository;
    protected bool isAdminMode;

    // ✅ Виправлений конструктор — залишаємо лише один!
    public UserUI(Repository<Player> repository, bool adminMode)
    {
        userRepository = repository;
        isAdminMode = adminMode;
        InitializeComponent();
        LoadData();
    }

    private void InitializeComponent()
    {
        dgvUsers = new DataGridView
        {
            Dock = DockStyle.Top,
            Height = 200,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };

        txtName = new TextBox
        {
            PlaceholderText = isAdminMode ? "Ім'я адміністратора" : "Ім'я гравця",
            Dock = DockStyle.Top
        };

        btnAdd = new Button
        {
            Text = isAdminMode ? "Додати адміністратора" : "Додати гравця",
            Dock = DockStyle.Left,
            Width = 150
        };

        btnEdit = new Button
        {
            Text = isAdminMode ? "Редагувати адміністратора" : "Редагувати гравця",
            Dock = DockStyle.Left,
            Width = 150
        };

        btnDelete = new Button
        {
            Text = isAdminMode ? "Видалити адміністратора" : "Видалити гравця",
            Dock = DockStyle.Left,
            Width = 150
        };

        btnAdd.Click += (s, e) => AddUser();
        btnEdit.Click += (s, e) => EditUser();
        btnDelete.Click += (s, e) => DeleteUser();

        Controls.Add(dgvUsers);
        Controls.Add(txtName);
        Controls.Add(btnAdd);
        Controls.Add(btnEdit);
        Controls.Add(btnDelete);
    }

    protected virtual void LoadData()
    {
        if (isAdminMode)
        {
            dgvUsers.DataSource = userRepository.GetAllAdmins()
                .Select(u => new { ID = u.Id, Name = u.Name })
                .ToList();
        }
        else
        {
            dgvUsers.DataSource = userRepository.GetAllPlayers()
                .Select(u => new { ID = u.Id, Name = u.Name })
                .ToList();
        }
    }

    protected virtual void AddUser()
    {
        if (!string.IsNullOrWhiteSpace(txtName.Text))
        {
            string newId = GenerateNextId();
            string name = isAdminMode ? $"{txtName.Text} [Admin Team]" : txtName.Text;

            if (isAdminMode)
            {
                userRepository.Add(new Admin(newId, name));
            }
            else
            {
                userRepository.Add(new Player(newId, name));
            }

            LoadData();
        }
    }

    protected virtual void EditUser()
    {
        if (dgvUsers.SelectedRows.Count > 0)
        {
            string id = dgvUsers.SelectedRows[0].Cells[0].Value.ToString();
            Player user = userRepository.GetById(id);

            if (user != null)
            {
                userRepository.Remove(user);

                string name = isAdminMode ? $"{txtName.Text} [Admin Team]" : txtName.Text;

                if (isAdminMode)
                {
                    userRepository.Add(new Admin(id, name));
                }
                else
                {
                    userRepository.Add(new Player(id, name));
                }

                LoadData();
            }
        }
    }

    protected virtual void DeleteUser()
    {
        if (dgvUsers.SelectedRows.Count > 0)
        {
            string id = dgvUsers.SelectedRows[0].Cells[0].Value.ToString();
            Player user = userRepository.GetById(id);

            if (user != null)
            {
                userRepository.Remove(user);
                LoadData();
            }
        }
    }

    private string GenerateNextId()
    {
        if (isAdminMode)
        {
            char nextId = 'A';
            while (userRepository.GetAllAdmins().Any(a => a.Id == nextId.ToString()))
            {
                nextId++;
            }
            return nextId.ToString();
        }
        else
        {
            int nextId = userRepository.GetAllPlayers()
                .Select(p => int.Parse(p.Id))
                .DefaultIfEmpty(0)
                .Max() + 1;

            return nextId.ToString();
        }
    }
}
