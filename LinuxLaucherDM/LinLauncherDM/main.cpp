#include "loadingwindow.h"

#include <QApplication>

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    LoadingWindow loadWindow;
    loadWindow.setWindowFlags(Qt::Window | Qt::FramelessWindowHint); // задал флаг, что без рамок окно
    loadWindow.show();
    return a.exec();
}
