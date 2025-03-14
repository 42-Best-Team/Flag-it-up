using System;
using System.Linq;
using System.Windows.Forms;

public partial class FlagUI : Form
{
    private Repository<Player> repository;
    private DataGridView dgvFlags;
    private TextBox txtCountry;
    private TextBox txtFlag;
    private Button btnAdd;
    private Button btnEdit;
    private Button btnDelete;

    public FlagUI(Repository<Player> repo)
    {
        repository = repo;
        InitializeComponent();
        LoadFlags();
    }

    private void InitializeComponent()
    {
        dgvFlags = new DataGridView
        {
            Dock = DockStyle.Top,
            Height = 200,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ReadOnly = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };

        txtCountry = new TextBox
        {
            PlaceholderText = "Країна",
            Dock = DockStyle.Top
        };

        txtFlag = new TextBox
        {
            PlaceholderText = "Прапор",
            Dock = DockStyle.Top
        };

        btnAdd = new Button
        {
            Text = "Додати прапор",
            Dock = DockStyle.Left,
            Width = 150
        };

        btnEdit = new Button
        {
            Text = "Редагувати прапор",
            Dock = DockStyle.Left,
            Width = 150
        };

        btnDelete = new Button
        {
            Text = "Видалити прапор",
            Dock = DockStyle.Left,
            Width = 150
        };

        btnAdd.Click += (s, e) => AddFlag();
        btnEdit.Click += (s, e) => EditFlag();
        btnDelete.Click += (s, e) => DeleteFlag();
        dgvFlags.SelectionChanged += (s, e) => LoadSelectedFlag();

        Controls.Add(dgvFlags);
        Controls.Add(txtCountry);
        Controls.Add(txtFlag);
        Controls.Add(btnAdd);
        Controls.Add(btnEdit);
        Controls.Add(btnDelete);
    }

    private void LoadFlags()
    {
        dgvFlags.DataSource = repository.GetAllFlags()
            .Select(f => new { Країна = f.Key, Прапор = f.Value })
            .ToList();
    }

    private void LoadSelectedFlag()
    {
        if (dgvFlags.SelectedRows.Count > 0)
        {
            txtCountry.Text = dgvFlags.SelectedRows[0].Cells[0].Value.ToString();
            txtFlag.Text = dgvFlags.SelectedRows[0].Cells[1].Value.ToString();
        }
    }

    private void AddFlag()
    {
        if (!string.IsNullOrWhiteSpace(txtCountry.Text) &&
            !string.IsNullOrWhiteSpace(txtFlag.Text))
        {
            repository.AddFlag(txtCountry.Text, txtFlag.Text);
            LoadFlags();
        }
    }

    private void EditFlag()
    {
        if (dgvFlags.SelectedRows.Count > 0)
        {
            string oldCountry = dgvFlags.SelectedRows[0].Cells[0].Value.ToString();
            string newCountry = txtCountry.Text;
            string newFlag = txtFlag.Text;

            if (!string.IsNullOrWhiteSpace(newCountry) &&
                !string.IsNullOrWhiteSpace(newFlag))
            {
                repository.RemoveFlag(oldCountry); // Видаляємо стару країну
                repository.AddFlag(newCountry, newFlag); // Додаємо нову пару
                LoadFlags();
            }
        }
    }

    private void DeleteFlag()
    {
        if (dgvFlags.SelectedRows.Count > 0)
        {
            string country = dgvFlags.SelectedRows[0].Cells[0].Value.ToString();
            repository.RemoveFlag(country);
            LoadFlags();
        }
    }
}
