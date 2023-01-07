Imports System.Data.SqlClient

Public Class Form_Login
    Private flag As Boolean = False 'Dung kiem soat timer
    Public LoginLoaiND As String = ""
    Public LoginTenND As String = ""

#Region "Event form load da hoan tat nhung chua hay"
    Private Sub Form_Login_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Tao_ket_noi()
        Kiem_tra_ket_noi()
    End Sub
#End Region

#Region "Kiem tra trang thai ket noi da hoan tat"
    Private Sub Kiem_tra_ket_noi()
        Try
            connect.Open()
            If connect.State = ConnectionState.Open Then
                connect.Close()
            End If
        Catch ex As Exception
            MessageBox.Show("Kết nối đến cơ sở dữ liệu không thành công...")
            Me.Close()
        End Try
    End Sub
#End Region

#Region "Hàm kiêm tra tinh hop le cua ket noi"
    Function Logged(ByVal U As String, ByVal P As String) As Integer
        Dim strSQL As String = "select IdNguoiDung, PassND, IdLoaiND from NguoiDung where IdNguoiDung = '" + U + "' "
        Dim Command As SqlCommand = New SqlCommand(strSQL, connect)
        connect.Open()
        'dien du lieu nguon vao doi tuong SQLDataReader
        Dim DataReader As SqlDataReader = Command.ExecuteReader()
        'Neu ton tai mau tin
        If DataReader.Read() Then
            'So sanh password
            If P = DataReader.GetString(1) Then
                'Nếu username và password đều hợp le
                'Dang nhap thanh cong
                Logged = 0
                LoginLoaiND = DataReader.GetValue(2).ToString
                LoginTenND = DataReader.GetValue(0).ToString
            Else
                'Sai pass và trả về giá trị -1
                Logged = -1
            End If
        Else
            'Khong tim thay username trong CSDL, trả về -2
            Logged = -2
        End If
        DataReader.Close()
        connect.Close()
    End Function
#End Region

#Region "Event Login_Click da hoan tat"
    Private Sub cmdLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLogin.Click
        If txtUserName.Text = "" Then
            MsgBox("Please enter your Username")
            txtUserName.Focus()
            Exit Sub
        End If
        'Yeu cau nguoi su dung nhap lai pass
        If txtPassword.Text = "" Then
            MsgBox("Please enter your pass")
            txtPassword.Focus()
            Exit Sub
        End If
        'Goi ham kiem tra username va pass
        Dim x As Integer = Logged(txtUserName.Text, txtPassword.Text)
        If x = -1 Then
            MsgBox("Bạn nhập sai password")
            txtPassword.Text = ""
            txtPassword.Focus()
        ElseIf x = -2 Then
            MsgBox("username không tồn tại")
            txtUserName.Text = ""
            txtUserName.Focus()
        Else
            flag = True
        End If
        If flag Then
            flag = False
            Me.Visible = False
            Form_Main.Show()
            Form_Main.WindowState = FormWindowState.Maximized
        End If
    End Sub
#End Region

#Region "Event Timer_Tick da hoan tat"
    Private Sub TimerClosing_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) 
        Me.Opacity -= 0.05
        If Me.Opacity = 0 Then
            flag = False
            Me.Visible = False
            Form_Main.Show()
            Form_Main.WindowState = FormWindowState.Maximized
        End If
    End Sub
#End Region

#Region "Event FormClosing da hoan tat"
    Private Sub FormLogin_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If flag Then
            e.Cancel() = True
        End If
    End Sub
#End Region

#Region "Su kien Link_Click da hoan tat"
    Private Sub LinkLabel_Language_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        If My.Settings.CultureString = "en-US" Then
            My.Settings.CultureString = "vi-VN"
        Else
            My.Settings.CultureString = "en-US"
        End If
        My.Settings.Save()
        Application.Restart()
    End Sub
#End Region

#Region "Event exit hoan tat"
    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Try
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Lỗi đóng chương trình")
        End Try
    End Sub
#End Region

End Class
