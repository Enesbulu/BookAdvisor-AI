import { useEffect, useState } from "react";
import { ReadingListService, type ReadingListDto } from "../services/readingList.service";
import { authService } from "../services/auth.service";
import { Alert, Button, Card, Container, Group, Loader, SimpleGrid, Text, Title } from "@mantine/core";
import { useNavigate } from "react-router-dom";
import { IconBook, IconLogout } from "@tabler/icons-react";


export function HomePage() {
    const navigate = useNavigate();
    const [lists, setLists] = useState<ReadingListDto[]>([]);   //backend dengelen listeleri tutar
    const [loading, setLoading] = useState(true);   //veri çekme sırasında loading simgesi göstermek için

    //sayfa ilk açıldığında bir kez çalışır.
    useEffect(() => {
        fetchLists();
    }, []);

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

    return (
        <Container size="lg" py="xl">
            {/* Üst Başlık ve Çıkış Butonu */}
            <Group justify="space-between" mb="xl">
                <Title order={2}>Kütüphanem</Title>
                <Button color="red" variant="light" leftSection={<IconLogout size={18} />} onClick={handleLogout}>Çıkış Yap</Button>
            </Group>
            {/* Yükleniyor durumu */}
            {loading ? (
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
            )}
        </Container>
    );
}