import { TextInput, PasswordInput, Checkbox, Anchor, Paper, Title, Text, Container, Group, Button } from "@mantine/core";
import { useForm } from '@mantine/form';
import { useNavigate } from "react-router-dom"; //sayfa yönlendirme için
import { authService } from "../services/auth.service";
import { useState } from "react";


export function LoginPage() {
    const form = useForm({
        initialValues: {
            email: '',
            password: '',
            rememberMe: false,
        },
        validate: {     //validasyon kuralları
            email: (value) => (/^\S+@\S+$/.test(value) ? null : 'Geçersiz email formatı'),
            password: (value) => (value.length < 6 ? 'şifre en az 6 karakter olmalıdır' : null),
        },
    });

    const navigate = useNavigate(); //yönlendirme 
    const [error, setError] = useState<string | null>(null);    //hata mesajı için değişken

    //Form gönderilince çalışacak fonksiyon
    const handleSubmit = async (values: typeof form.values) => {
        setError(null);
        try {
            //istek at -login
            const response = await authService.Login(values);
            //gelen response dan token al, localStorage a kaydet.
            localStorage.setItem('token', response.token);
            //console a vilgi basma
            console.log("Giriş başarılı Token: ", response.token);
            //yönlendirme
            navigate("/HomePage");
            alert("Giriş Başarılı!");

        } catch (err) {
            console.error("Giriş başarısız: ", err);
            setError("Giriş Başarısız. ")
        }
    };

    return (
        <Container size={420} my={40}>
            <Title ta="center">Hoş Geldiniz</Title>
            <Text c="dimmed" size="sm" ta="center" mt={5}>Hesabınız yok mu?{' '}
                <Anchor size="sm" component="button">Kayıt ol</Anchor>
            </Text>

            {/* Kart görünümü için Paper */}
            <Paper withBorder shadow="md" p={30} mt={30} radius="md">
                {/* Form gönderildiğinde çalışacak kısım */}
                <form onSubmit={form.onSubmit(handleSubmit)} noValidate>
                    {error && (
                        <Text c="red" size="sm" ta="center" mb="md">{error}</Text>
                    )}
                    <TextInput label="Email" placeholder="ornek@mail.com" required
                        {...form.getInputProps('email')} //Bu satır, input ile form değişkenini birbirine bağlar. yazdıkça değişken güncellenir, hata varsa ekrana basar.
                    />
                    <PasswordInput label="Şifre" placeholder="Şifreniz" required mt="md"
                        {...form.getInputProps('password')} //password alanı ile form yapısını birbirine bağlar, yazma sırasında hata denetimi yapar ve ekrana basar
                    />

                    <Group justify="space-between" mt="lg">
                        <Checkbox label="Beni hatırla" />
                        <Anchor component="button" size="sm">
                            Şifremi unuttum
                        </Anchor>
                    </Group>
                    <Button fullWidth mt="xl" type="submit">Giriş Yap</Button>
                </form>
            </Paper>
        </Container>
    );
}




