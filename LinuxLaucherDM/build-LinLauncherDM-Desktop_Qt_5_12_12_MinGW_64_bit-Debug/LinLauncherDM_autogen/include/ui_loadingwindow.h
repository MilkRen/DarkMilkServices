/********************************************************************************
** Form generated from reading UI file 'loadingwindow.ui'
**
** Created by: Qt User Interface Compiler version 5.12.12
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_LOADINGWINDOW_H
#define UI_LOADINGWINDOW_H

#include <QtCore/QVariant>
#include <QtWidgets/QApplication>
#include <QtWidgets/QPushButton>
#include <QtWidgets/QWidget>

QT_BEGIN_NAMESPACE

class Ui_LoadingWindow
{
public:
    QPushButton *pushButton;

    void setupUi(QWidget *LoadingWindow)
    {
        if (LoadingWindow->objectName().isEmpty())
            LoadingWindow->setObjectName(QString::fromUtf8("LoadingWindow"));
        LoadingWindow->resize(500, 100);
        LoadingWindow->setStyleSheet(QString::fromUtf8("QWidget\n"
"{\n"
" background-color: Black;\n"
"}"));
        pushButton = new QPushButton(LoadingWindow);
        pushButton->setObjectName(QString::fromUtf8("pushButton"));
        pushButton->setGeometry(QRect(130, 190, 80, 21));

        retranslateUi(LoadingWindow);

        QMetaObject::connectSlotsByName(LoadingWindow);
    } // setupUi

    void retranslateUi(QWidget *LoadingWindow)
    {
        LoadingWindow->setWindowTitle(QApplication::translate("LoadingWindow", "DarkMilk", nullptr));
        pushButton->setText(QApplication::translate("LoadingWindow", "PushButton", nullptr));
    } // retranslateUi

};

namespace Ui {
    class LoadingWindow: public Ui_LoadingWindow {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_LOADINGWINDOW_H
