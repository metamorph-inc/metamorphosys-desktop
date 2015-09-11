/*
 * Greybus manifest definition
 *
 * See "Greybus Application Protocol" document (version 0.1) for
 * details on these values and structures.
 *
 * Copyright 2014-2015 Google Inc.
 * Copyright 2014-2015 Linaro Ltd.
 *
 * Released under the GPLv2 and BSD licenses.
 */

#ifndef __GREYBUS_MANIFEST_H
#define __GREYBUS_MANIFEST_H

#ifdef _WIN32
#pragma warning(disable : 4200)
#endif

enum greybus_descriptor_type {
    GREYBUS_TYPE_INVALID = 0x00,
    GREYBUS_TYPE_INTERFACE = 0x01,
    GREYBUS_TYPE_STRING = 0x02,
    GREYBUS_TYPE_BUNDLE = 0x03,
    GREYBUS_TYPE_CPORT = 0x04,
};

enum greybus_protocol {
    GREYBUS_PROTOCOL_CONTROL = 0x00,
    GREYBUS_PROTOCOL_AP = 0x01,
    GREYBUS_PROTOCOL_GPIO = 0x02,
    GREYBUS_PROTOCOL_I2C = 0x03,
    GREYBUS_PROTOCOL_UART = 0x04,
    GREYBUS_PROTOCOL_HID = 0x05,
    GREYBUS_PROTOCOL_USB = 0x06,
    GREYBUS_PROTOCOL_SDIO = 0x07,
    GREYBUS_PROTOCOL_BATTERY = 0x08,
    GREYBUS_PROTOCOL_PWM = 0x09,
    GREYBUS_PROTOCOL_I2S_MGMT = 0x0a,
    GREYBUS_PROTOCOL_SPI = 0x0b,
    GREYBUS_PROTOCOL_DISPLAY = 0x0c,
    GREYBUS_PROTOCOL_CAMERA = 0x0d,
    GREYBUS_PROTOCOL_SENSOR = 0x0e,
    GREYBUS_PROTOCOL_LIGHTS = 0x0f,
    GREYBUS_PROTOCOL_VIBRATOR = 0x10,
    GREYBUS_PROTOCOL_LOOPBACK = 0x11,
    GREYBUS_PROTOCOL_I2S_RECEIVER = 0x12,
    GREYBUS_PROTOCOL_I2S_TRANSMITTER = 0x13,
    GREYBUS_PROTOCOL_SVC = 0x14,
    /* ... */
    GREYBUS_PROTOCOL_RAW = 0xfe,
    GREYBUS_PROTOCOL_VENDOR = 0xff,

    GREYBUS_PROTOCOL_UNKNOWN = 0x37 // arbitrary value
};

enum greybus_class_type {
    GREYBUS_CLASS_CONTROL = 0x00,
    GREYBUS_CLASS_AP = 0x01,
    GREYBUS_CLASS_GPIO = 0x02,
    GREYBUS_CLASS_I2C = 0x03,
    GREYBUS_CLASS_UART = 0x04,
    GREYBUS_CLASS_HID = 0x05,
    GREYBUS_CLASS_USB = 0x06,
    GREYBUS_CLASS_SDIO = 0x07,
    GREYBUS_CLASS_BATTERY = 0x08,
    GREYBUS_CLASS_PWM = 0x09,
    GREYBUS_CLASS_I2S = 0x0a,
    GREYBUS_CLASS_SPI = 0x0b,
    GREYBUS_CLASS_DISPLAY = 0x0c,
    GREYBUS_CLASS_CAMERA = 0x0d,
    GREYBUS_CLASS_SENSOR = 0x0e,
    GREYBUS_CLASS_LIGHTS = 0x0f,
    GREYBUS_CLASS_VIBRATOR = 0x10,
    GREYBUS_CLASS_LOOPBACK = 0x11,
    GREYBUS_CLASS_VENDOR = 0xff,
};

#ifdef _WIN32
#define PACKED
#pragma pack(push, 1)
#else
#define PACKED __attribute__((packed))
#endif
/*
 * The string in a string descriptor is not NUL-terminated.  The
 * size of the descriptor will be rounded up to a multiple of 4
 * bytes, by padding the string with 0x00 bytes if necessary.
 */
struct PACKED greybus_descriptor_string {
    uint8_t length;
    uint8_t id;
    uint8_t string[0];
};

/*
 * An interface descriptor describes information about an interface as a whole,
 * *not* the functions within it.
 */
struct PACKED greybus_descriptor_interface {
    uint8_t vendor_stringid;
    uint8_t product_stringid;
    uint8_t pad[2];
};

/*
 * An bundle descriptor defines an identification number and a class for
 * each bundle.
 *
 * @id: Uniquely identifies a bundle within a interface, its sole purpose is to
 * allow CPort descriptors to specify which bundle they are associated with.
 * The first bundle will have id 0, second will have 1 and so on.
 *
 * The largest CPort id associated with an bundle (defined by a
 * CPort descriptor in the manifest) is used to determine how to
 * encode the device id and module number in UniPro packets
 * that use the bundle.
 *
 * @class: It is used by kernel to know the functionality provided by the
 * bundle and will be matched against drivers functinality while probing greybus
 * driver. It should contain one of the values defined in
 * 'enum greybus_class_type'.
 *
 */
struct PACKED greybus_descriptor_bundle {
    uint8_t id; /* interface-relative id (0..) */
    uint8_t protocol_class;
    uint8_t pad[2];
};

/*
 * A CPort descriptor indicates the id of the bundle within the
 * module it's associated with, along with the CPort id used to
 * address the CPort.  The protocol id defines the format of messages
 * exchanged using the CPort.
 */
struct PACKED greybus_descriptor_cport {
    uint16_t id;
    uint8_t bundle;
    uint8_t protocol_id; /* enum greybus_protocol */
};

struct PACKED greybus_descriptor_header {
    uint16_t size;
    uint8_t type; /* enum greybus_descriptor_type */
    uint8_t pad;
};

struct PACKED greybus_descriptor {
    struct greybus_descriptor_header header;
    union {
        struct greybus_descriptor_string string;
        struct greybus_descriptor_interface interface_;
        struct greybus_descriptor_bundle bundle;
        struct greybus_descriptor_cport cport;
    };
};

struct PACKED greybus_manifest_header {
    uint16_t size;
    uint8_t version_major;
    uint8_t version_minor;
};

struct PACKED greybus_manifest {
    struct greybus_manifest_header header;
    struct greybus_descriptor descriptors[0];
};

#ifdef _WIN32
#pragma pack(pop)
#undef PACKED
#else
#undef PACKED
#endif

#ifdef _WIN32
#pragma warning(default : 4200)
#endif

#endif /* __GREYBUS_MANIFEST_H */
