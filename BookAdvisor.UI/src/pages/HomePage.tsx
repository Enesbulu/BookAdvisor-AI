import { useEffect, useState } from "react";
import { ReadingListService, type ReadingListDto } from "../services/readingList.service";
import { authService } from "../services/auth.service";
import { Alert, Button, Card, Container, Group, Loader, Modal, Notification, SimpleGrid, Text, TextInput, Title } from "@mantine/core";
import { useNavigate } from "react-router-dom";
import { IconBook, IconCheck, IconLogout, IconPlus } from "@tabler/icons-react";
import { useDisclosure } from "@mantine/hooks";
import { useForm } from "@mantine/form";


export function HomePage() {
    const navigate = useNavigate();
    const [lists, setLists] = useState<ReadingListDto[]>([]);   //backend dengelen listeleri tutar
    const [loading, setLoading] = useState(true);   //veri çekme sırasında loading simgesi göstermek için

    //Modal Kontrolü (useDisclosure)
    // opened: Modal açık mı? (true/false)
    // open: Modalı açan fonksiyon
    // close: Modalı kapatan fonksiyon
    const [opened, { open, close }] = useDisclosure(false);
    const [showSuccess, setShowSuccess] = useState(false);  // Bildirim göstermek için state

    //Yeni liste oluşturma formu
    const form = useForm({
        initialValues: { name: '' },
        validate: {
            name: (value) => (value.length < 3 ? 'Liste adı en az 3 karakter olmalıdır.' : null)
        },
    });

    //sayfa ilk açıldığında bir kez çalışır.
    useEffect(() => {
        fetchLists();
    }, []);

    ///Bütün okumaListesi ni çekme-listeleme işlemi için metod 
    const fetchLists = async () => {
        try {
            const data = await ReadingListService.getAll();
            setLists(data.data);
        } catch (err) {
            console.error("Listeler çekilemedi", err);
        }
        finally {
            setLoading(false);  //her şekilde yükleniyor'u kapat
        }
    };

    //oturum kapatma kontrolü
    const handleLogout = () => {
        authService.logout();
        navigate('/login');
    };

    //Yeni liste ekleme fonksiyonu
    const handleCreateList = async (value: typeof form.values) => {
        try {
            //backend'e gönderme
            await ReadingListService.create({ name: value.name });

            //modal kapatma
            close();
            form.reset();

            //Başarı mesajı gösterme, süreli olarak
            setShowSuccess(true);
            setTimeout(() => setShowSuccess(false), 3000);  //3 sn sonra kapat

            //Listeyi yenile
            fetchLists();

        } catch (err) {
            console.error("Liste oluşturulurken bir hata gerçekleşti: ", err);
            alert("Hata oluştu!");
        }
    }

    return (
        <Container size="lg" py="xl">


            {/* Modal Bileşeni */}
            <Modal opened={opened} onClose={close} title="Yeni Okuma Listesi Oluşturma" centered>
                <form onSubmit={form.onSubmit(handleCreateList)}>
                    <TextInput label="Liste Adı" placeholder="Örn: Yaz Tatlili Kitapları" data-autofocus required {...form.getInputProps('name')} />

                    <Group justify="flex-end" mt="md">
                        <Button variant="default" onClick={close}>İptal</Button>
                        <Button type="submit" >Oluştur</Button>
                    </Group>
                </form>
            </Modal>

            {/* Form oluşturma başarı bildirimi */}
            {showSuccess && (
                <Notification icon={<IconCheck size={18} />} color="teal" title="Başarılı" onClose={() => setShowSuccess(false)} mb="md">
                    Yeni Liste Başarıyla Oluşturuldu.
                </Notification >
            )}

            {/* Üst Başlık ve Çıkış Butonu */}
            <Group justify="space-between" mb="xl">
                <Title order={2}>Kütüphanem</Title>
                <Group>
                    <Button leftSection={<IconPlus size={18} />} onClick={open}>Yeni Liste</Button>
                    <Button color="red" variant="light" leftSection={<IconLogout size={18} />} onClick={handleLogout}>Çıkış Yap</Button>
                </Group>
            </Group>
            {/* Yükleniyor durumu */}
            {
                loading ? (
                    <Group justify="center">
                        <Loader size="lg" />
                    </Group>
                ) : (
                    <>
                        {/* Liste Boşsa Uyarı Göster */}
                        {lists.length === 0 ? (
                            <Alert title="Listeniz Boş" color="blue">Henüz hiç okuma listesi oluşturmadınız.</Alert>

                        ) : (
                            //MAP fonksiyonu
                            <SimpleGrid cols={{ base: 1, sm: 2, lg: 3 }}>
                                {lists.map((list) => (
                                    <Card key={list.id} shadow="sm" padding="lg" radius="md" withBorder>
                                        <Group justify="space-between" mt="md" mb="xs">
                                            <Text fw={500}>{list.name}</Text>
                                            <IconBook size={20} color="gray" />
                                        </Group>
                                        <Text size="sm" c="dimmed">Bu liste {list.bookCount} adet kitap içerir.</Text>

                                        <Button color="blue" fullWidth mt="md" radius="md">Detayları gör</Button>
                                    </Card>
                                ))}
                            </SimpleGrid>
                        )}
                    </>
                )
            }
        </Container >
    );
}