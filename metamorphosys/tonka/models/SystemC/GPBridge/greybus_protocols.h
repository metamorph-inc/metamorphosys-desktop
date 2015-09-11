/*
 * This file is provided under a dual BSD/GPLv2 license.  When using or
 * redistributing this file, you may do so under either license.
 *
 * GPL LICENSE SUMMARY
 *
 * Copyright(c) 2014 - 2015 Google Inc. All rights reserved.
 * Copyright(c) 2014 - 2015 Linaro Ltd. All rights reserved.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of version 2 of the GNU General Public License as
 * published by the Free Software Foundation.
 *
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License version 2 for more details.
 *
 * BSD LICENSE
 *
 * Copyright(c) 2014 - 2015 Google Inc. All rights reserved.
 * Copyright(c) 2014 - 2015 Linaro Ltd. All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 *
 *  * Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 *  * Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in
 *    the documentation and/or other materials provided with the
 *    distribution.
 *  * Neither the name of Google Inc. or Linaro Ltd. nor the names of
 *    its contributors may be used to endorse or promote products
 *    derived from this software without specific prior written
 *    permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 * A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL GOOGLE INC. OR
 * LINARO LTD. BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY
 * OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

#ifndef __GREYBUS_PROTOCOLS_H
#define __GREYBUS_PROTOCOLS_H

#ifdef _WIN32
// C4200: nonstandard extension used : zero-sized array in struct/union
#pragma warning(disable : 4200)
#endif

/* Fixed IDs for control/svc protocols */

/* Device ID of SVC and AP */
#define GB_DEVICE_ID_SVC 0
#define GB_DEVICE_ID_AP 1
#define GB_DEVICE_ID_MODULES_START 2

/*
 * Bundle/cport for control/svc cport: The same bundle/cport is shared by both
 * CONTROL and SVC protocols for communication between AP and SVC.
 */
#define GB_SVC_BUNDLE_ID 0
#define GB_SVC_CPORT_ID 0
#define GB_CONTROL_BUNDLE_ID 0
#define GB_CONTROL_CPORT_ID 0

/* Control Protocol */

/* version request has no payload */
struct gb_protocol_version_response {
    uint8_t major;
    uint8_t minor;
};

/* Control Protocol */

/* Version of the Greybus control protocol we support */
#define GB_CONTROL_VERSION_MAJOR 0x00
#define GB_CONTROL_VERSION_MINOR 0x01

/* Greybus control request types */
#define GB_CONTROL_TYPE_INVALID 0x00
#define GB_CONTROL_TYPE_PROTOCOL_VERSION 0x01
#define GB_CONTROL_TYPE_PROBE_AP 0x02
#define GB_CONTROL_TYPE_GET_MANIFEST_SIZE 0x03
#define GB_CONTROL_TYPE_GET_MANIFEST 0x04
#define GB_CONTROL_TYPE_CONNECTED 0x05
#define GB_CONTROL_TYPE_DISCONNECTED 0x06

/* Control protocol manifest get size request has no payload*/
struct gb_control_get_manifest_size_response {
    uint16_t size;
};

/* Control protocol manifest get request has no payload */
struct gb_control_get_manifest_response {
    uint8_t data[0];
};

/* Control protocol [dis]connected request */
struct gb_control_connected_request {
    uint16_t cport_id;
};

struct gb_control_disconnected_request {
    uint16_t cport_id;
};
/* Control protocol [dis]connected response has no payload */

/* I2C */

/* Version of the Greybus i2c protocol we support */
#define GB_I2C_VERSION_MAJOR 0x00
#define GB_I2C_VERSION_MINOR 0x01

/* Greybus i2c request types */
#define GB_I2C_TYPE_INVALID 0x00
#define GB_I2C_TYPE_PROTOCOL_VERSION 0x01
#define GB_I2C_TYPE_FUNCTIONALITY 0x02
#define GB_I2C_TYPE_TIMEOUT 0x03
#define GB_I2C_TYPE_RETRIES 0x04
#define GB_I2C_TYPE_TRANSFER 0x05

#define GB_I2C_RETRIES_DEFAULT 3
#define GB_I2C_TIMEOUT_DEFAULT 1000 /* milliseconds */

/* functionality request has no payload */
struct gb_i2c_functionality_response {
    uint32_t functionality;
};

struct gb_i2c_timeout_request {
    uint16_t msec;
};
/* timeout response has no payload */

struct gb_i2c_retries_request {
    uint8_t retries;
};
/* retries response has no payload */

/*
 * Outgoing data immediately follows the op count and ops array.
 * The data for each write (master -> slave) op in the array is sent
 * in order, with no (e.g. pad) bytes separating them.
 *
 * Short reads cause the entire transfer request to fail So response
 * payload consists only of bytes read, and the number of bytes is
 * exactly what was specified in the corresponding op.  Like
 * outgoing data, the incoming data is in order and contiguous.
 */
struct gb_i2c_transfer_op {
    uint16_t addr;
    uint16_t flags;
    uint16_t size;
};

struct gb_i2c_transfer_request {
    uint16_t op_count;
    struct gb_i2c_transfer_op ops[0]; /* op_count of these */
};
struct gb_i2c_transfer_response {
    uint8_t data[0]; /* inbound data */
};

struct op_header {
    uint16_t size;
    uint16_t id;
    uint8_t type;
    uint8_t result;
    uint8_t pad[2];
};

/* common ops */
struct protocol_version_rsp {
    uint8_t version_major;
    uint8_t version_minor;
};

/* Ops */

/* GPIO */

/* Version of the Greybus GPIO protocol we support */
#define GB_GPIO_VERSION_MAJOR 0x00
#define GB_GPIO_VERSION_MINOR 0x01

/* Greybus GPIO request types */
#define GB_GPIO_TYPE_INVALID 0x00
#define GB_GPIO_TYPE_PROTOCOL_VERSION 0x01
#define GB_GPIO_TYPE_LINE_COUNT 0x02
#define GB_GPIO_TYPE_ACTIVATE 0x03
#define GB_GPIO_TYPE_DEACTIVATE 0x04
#define GB_GPIO_TYPE_GET_DIRECTION 0x05
#define GB_GPIO_TYPE_DIRECTION_IN 0x06
#define GB_GPIO_TYPE_DIRECTION_OUT 0x07
#define GB_GPIO_TYPE_GET_VALUE 0x08
#define GB_GPIO_TYPE_SET_VALUE 0x09
#define GB_GPIO_TYPE_SET_DEBOUNCE 0x0a
#define GB_GPIO_TYPE_IRQ_TYPE 0x0b
#define GB_GPIO_TYPE_IRQ_MASK 0x0c
#define GB_GPIO_TYPE_IRQ_UNMASK 0x0d
#define GB_GPIO_TYPE_IRQ_EVENT 0x0e

#define GB_GPIO_IRQ_TYPE_NONE 0x00
#define GB_GPIO_IRQ_TYPE_EDGE_RISING 0x01
#define GB_GPIO_IRQ_TYPE_EDGE_FALLING 0x02
#define GB_GPIO_IRQ_TYPE_EDGE_BOTH 0x03
#define GB_GPIO_IRQ_TYPE_LEVEL_HIGH 0x04
#define GB_GPIO_IRQ_TYPE_LEVEL_LOW 0x08

/* line count request has no payload */
struct gb_gpio_line_count_response {
    uint8_t count;
};

struct gb_gpio_activate_request {
    uint8_t which;
};
/* activate response has no payload */

struct gb_gpio_deactivate_request {
    uint8_t which;
};
/* deactivate response has no payload */

struct gb_gpio_get_direction_request {
    uint8_t which;
};
struct gb_gpio_get_direction_response {
    uint8_t direction;
};

struct gb_gpio_direction_in_request {
    uint8_t which;
};
/* direction in response has no payload */

struct gb_gpio_direction_out_request {
    uint8_t which;
    uint8_t value;
};
/* direction out response has no payload */

struct gb_gpio_get_value_request {
    uint8_t which;
};
struct gb_gpio_get_value_response {
    uint8_t value;
};

struct gb_gpio_set_value_request {
    uint8_t which;
    uint8_t value;
};
/* set value response has no payload */

struct gb_gpio_set_debounce_request {
    uint8_t which;
    uint16_t usec;
};
/* debounce response has no payload */

struct gb_gpio_irq_type_request {
    uint8_t which;
    uint8_t type;
};
/* irq type response has no payload */

struct gb_gpio_irq_mask_request {
    uint8_t which;
};
/* irq mask response has no payload */

struct gb_gpio_irq_unmask_request {
    uint8_t which;
};
/* irq unmask response has no payload */

/* irq event requests originate on another module and are handled on the AP */
struct gb_gpio_irq_event_request {
    uint8_t which;
};
/* irq event has no response */

/* PWM */

/* Version of the Greybus PWM protocol we support */
#define GB_PWM_VERSION_MAJOR 0x00
#define GB_PWM_VERSION_MINOR 0x01

/* Greybus PWM operation types */
#define GB_PWM_TYPE_INVALID 0x00
#define GB_PWM_TYPE_PROTOCOL_VERSION 0x01
#define GB_PWM_TYPE_PWM_COUNT 0x02
#define GB_PWM_TYPE_ACTIVATE 0x03
#define GB_PWM_TYPE_DEACTIVATE 0x04
#define GB_PWM_TYPE_CONFIG 0x05
#define GB_PWM_TYPE_POLARITY 0x06
#define GB_PWM_TYPE_ENABLE 0x07
#define GB_PWM_TYPE_DISABLE 0x08

/* pwm count request has no payload */
struct gb_pwm_count_response {
    uint8_t count;
};

struct gb_pwm_activate_request {
    uint8_t which;
};

struct gb_pwm_deactivate_request {
    uint8_t which;
};

struct gb_pwm_config_request {
    uint8_t which;
    uint32_t duty;
    uint32_t period;
};

struct gb_pwm_polarity_request {
    uint8_t which;
    uint8_t polarity;
};

struct gb_pwm_enable_request {
    uint8_t which;
};

struct gb_pwm_disable_request {
    uint8_t which;
};

/* I2S */

#define GB_I2S_MGMT_TYPE_PROTOCOL_VERSION 0x01
#define GB_I2S_MGMT_TYPE_GET_SUPPORTED_CONFIGURATIONS 0x02
#define GB_I2S_MGMT_TYPE_SET_CONFIGURATION 0x03
#define GB_I2S_MGMT_TYPE_SET_SAMPLES_PER_MESSAGE 0x04
#define GB_I2S_MGMT_TYPE_GET_PROCESSING_DELAY 0x05
#define GB_I2S_MGMT_TYPE_SET_START_DELAY 0x06
#define GB_I2S_MGMT_TYPE_ACTIVATE_CPORT 0x07
#define GB_I2S_MGMT_TYPE_DEACTIVATE_CPORT 0x08
#define GB_I2S_MGMT_TYPE_REPORT_EVENT 0x09

#define GB_I2S_MGMT_BYTE_ORDER_NA BIT(0)
#define GB_I2S_MGMT_BYTE_ORDER_BE BIT(1)
#define GB_I2S_MGMT_BYTE_ORDER_LE BIT(2)

#define GB_I2S_MGMT_SPATIAL_LOCATION_FL BIT(0)
#define GB_I2S_MGMT_SPATIAL_LOCATION_FR BIT(1)
#define GB_I2S_MGMT_SPATIAL_LOCATION_FC BIT(2)
#define GB_I2S_MGMT_SPATIAL_LOCATION_LFE BIT(3)
#define GB_I2S_MGMT_SPATIAL_LOCATION_BL BIT(4)
#define GB_I2S_MGMT_SPATIAL_LOCATION_BR BIT(5)
#define GB_I2S_MGMT_SPATIAL_LOCATION_FLC BIT(6)
#define GB_I2S_MGMT_SPATIAL_LOCATION_FRC BIT(7)
#define GB_I2S_MGMT_SPATIAL_LOCATION_C BIT(8) /* BC in USB */
#define GB_I2S_MGMT_SPATIAL_LOCATION_SL BIT(9)
#define GB_I2S_MGMT_SPATIAL_LOCATION_SR BIT(10)
#define GB_I2S_MGMT_SPATIAL_LOCATION_TC BIT(11)
#define GB_I2S_MGMT_SPATIAL_LOCATION_TFL BIT(12)
#define GB_I2S_MGMT_SPATIAL_LOCATION_TFC BIT(13)
#define GB_I2S_MGMT_SPATIAL_LOCATION_TFR BIT(14)
#define GB_I2S_MGMT_SPATIAL_LOCATION_TBL BIT(15)
#define GB_I2S_MGMT_SPATIAL_LOCATION_TBC BIT(16)
#define GB_I2S_MGMT_SPATIAL_LOCATION_TBR BIT(17)
#define GB_I2S_MGMT_SPATIAL_LOCATION_TFLC BIT(18)
#define GB_I2S_MGMT_SPATIAL_LOCATION_TFRC BIT(19)
#define GB_I2S_MGMT_SPATIAL_LOCATION_LLFE BIT(20)
#define GB_I2S_MGMT_SPATIAL_LOCATION_RLFE BIT(21)
#define GB_I2S_MGMT_SPATIAL_LOCATION_TSL BIT(22)
#define GB_I2S_MGMT_SPATIAL_LOCATION_TSR BIT(23)
#define GB_I2S_MGMT_SPATIAL_LOCATION_BC BIT(24)
#define GB_I2S_MGMT_SPATIAL_LOCATION_BLC BIT(25)
#define GB_I2S_MGMT_SPATIAL_LOCATION_BRC BIT(26)
#define GB_I2S_MGMT_SPATIAL_LOCATION_RD BIT(31)

#define GB_I2S_MGMT_PROTOCOL_PCM BIT(0)
#define GB_I2S_MGMT_PROTOCOL_I2S BIT(1)
#define GB_I2S_MGMT_PROTOCOL_LR_STEREO BIT(2)

#define GB_I2S_MGMT_ROLE_MASTER BIT(0)
#define GB_I2S_MGMT_ROLE_SLAVE BIT(1)

#define GB_I2S_MGMT_POLARITY_NORMAL BIT(0)
#define GB_I2S_MGMT_POLARITY_REVERSED BIT(1)

#define GB_I2S_MGMT_EDGE_RISING BIT(0)
#define GB_I2S_MGMT_EDGE_FALLING BIT(1)

#define GB_I2S_MGMT_EVENT_UNSPECIFIED 0x1
#define GB_I2S_MGMT_EVENT_HALT 0x2
#define GB_I2S_MGMT_EVENT_INTERNAL_ERROR 0x3
#define GB_I2S_MGMT_EVENT_PROTOCOL_ERROR 0x4
#define GB_I2S_MGMT_EVENT_FAILURE 0x5
#define GB_I2S_MGMT_EVENT_OUT_OF_SEQUENCE 0x6
#define GB_I2S_MGMT_EVENT_UNDERRUN 0x7
#define GB_I2S_MGMT_EVENT_OVERRUN 0x8
#define GB_I2S_MGMT_EVENT_CLOCKING 0x9
#define GB_I2S_MGMT_EVENT_DATA_LEN 0xa

struct gb_i2s_mgmt_configuration {
    uint32_t sample_frequency;
    uint8_t num_channels;
    uint8_t bytes_per_channel;
    uint8_t byte_order;
    uint8_t pad;
    uint32_t spatial_locations;
    uint32_t ll_protocol;
    uint8_t ll_mclk_role;
    uint8_t ll_bclk_role;
    uint8_t ll_wclk_role;
    uint8_t ll_wclk_polarity;
    uint8_t ll_wclk_change_edge;
    uint8_t ll_wclk_tx_edge;
    uint8_t ll_wclk_rx_edge;
    uint8_t ll_data_offset;
};

/* get supported configurations request has no payload */
struct gb_i2s_mgmt_get_supported_configurations_response {
    uint8_t config_count;
    uint8_t pad[3];
    struct gb_i2s_mgmt_configuration config[0];
};

struct gb_i2s_mgmt_set_configuration_request {
    struct gb_i2s_mgmt_configuration config;
};
/* set configuration response has no payload */

struct gb_i2s_mgmt_set_samples_per_message_request {
    uint16_t samples_per_message;
};
/* set samples per message response has no payload */

/* get processing request delay has no payload */
struct gb_i2s_mgmt_get_processing_delay_response {
    uint32_t microseconds;
};

struct gb_i2s_mgmt_set_start_delay_request {
    uint32_t microseconds;
};
/* set start delay response has no payload */

struct gb_i2s_mgmt_activate_cport_request {
    uint16_t cport;
};
/* activate cport response has no payload */

struct gb_i2s_mgmt_deactivate_cport_request {
    uint16_t cport;
};
/* deactivate cport response has no payload */

struct gb_i2s_mgmt_report_event_request {
    uint8_t event;
};
/* report event response has no payload */

#define GB_I2S_DATA_TYPE_PROTOCOL_VERSION 0x01
#define GB_I2S_DATA_TYPE_SEND_DATA 0x02

struct gb_i2s_send_data_request {
    uint32_t sample_number;
    uint32_t size;
    uint8_t data[0];
};
/* send data has no response at all */

/* SPI */

/* Version of the Greybus spi protocol we support */
#define GB_SPI_VERSION_MAJOR 0x00
#define GB_SPI_VERSION_MINOR 0x01

/* Should match up with modes in linux/spi/spi.h */
#define GB_SPI_MODE_CPHA 0x01 /* clock phase */
#define GB_SPI_MODE_CPOL 0x02 /* clock polarity */
#define GB_SPI_MODE_MODE_0 (0 | 0) /* (original MicroWire) */
#define GB_SPI_MODE_MODE_1 (0 | GB_SPI_MODE_CPHA)
#define GB_SPI_MODE_MODE_2 (GB_SPI_MODE_CPOL | 0)
#define GB_SPI_MODE_MODE_3 (GB_SPI_MODE_CPOL | GB_SPI_MODE_CPHA)
#define GB_SPI_MODE_CS_HIGH 0x04 /* chipselect active high? */
#define GB_SPI_MODE_LSB_FIRST 0x08 /* per-word bits-on-wire */
#define GB_SPI_MODE_3WIRE 0x10 /* SI/SO signals shared */
#define GB_SPI_MODE_LOOP 0x20 /* loopback mode */
#define GB_SPI_MODE_NO_CS 0x40 /* 1 dev/bus, no chipselect */
#define GB_SPI_MODE_READY 0x80 /* slave pulls low to pause */

/* Should match up with flags in linux/spi/spi.h */
#define GB_SPI_FLAG_HALF_DUPLEX BIT(0) /* can't do full duplex */
#define GB_SPI_FLAG_NO_RX BIT(1) /* can't do buffer read */
#define GB_SPI_FLAG_NO_TX BIT(2) /* can't do buffer write */

/* Greybus spi operation types */
#define GB_SPI_TYPE_INVALID 0x00
#define GB_SPI_TYPE_PROTOCOL_VERSION 0x01
#define GB_SPI_TYPE_MODE 0x02
#define GB_SPI_TYPE_FLAGS 0x03
#define GB_SPI_TYPE_BITS_PER_WORD_MASK 0x04
#define GB_SPI_TYPE_NUM_CHIPSELECT 0x05
#define GB_SPI_TYPE_TRANSFER 0x06

/* mode request has no payload */
struct gb_spi_mode_response {
    uint16_t mode;
};

/* flags request has no payload */
struct gb_spi_flags_response {
    uint16_t flags;
};

/* bits-per-word request has no payload */
struct gb_spi_bpw_response {
    uint32_t bits_per_word_mask;
};

/* num-chipselects request has no payload */
struct gb_spi_chipselect_response {
    uint16_t num_chipselect;
};

/**
 * struct gb_spi_transfer - a read/write buffer pair
 * @speed_hz: Select a speed other than the device default for this transfer. If
 *	0 the default (from @spi_device) is used.
 * @len: size of rx and tx buffers (in bytes)
 * @delay_usecs: microseconds to delay after this transfer before (optionally)
 * 	changing the chipselect status, then starting the next transfer or
 * 	completing this spi_message.
 * @cs_change: affects chipselect after this transfer completes
 * @bits_per_word: select a bits_per_word other than the device default for this
 *	transfer. If 0 the default (from @spi_device) is used.
 */
struct gb_spi_transfer {
    uint32_t speed_hz;
    uint32_t len;
    uint16_t delay_usecs;
    uint8_t cs_change;
    uint8_t bits_per_word;
};

struct gb_spi_transfer_request {
    uint8_t chip_select; /* of the spi device */
    uint8_t mode; /* of the spi device */
    uint16_t count;
    struct gb_spi_transfer transfers[0]; /* trnasfer_count of these */
};

struct gb_spi_transfer_response {
    uint8_t data[0]; /* inbound data */
};

/* Version of the Greybus SVC protocol we support */
#define GB_SVC_VERSION_MAJOR 0x00
#define GB_SVC_VERSION_MINOR 0x01

/* Greybus SVC request types */
#define GB_SVC_TYPE_INVALID 0x00
#define GB_SVC_TYPE_PROTOCOL_VERSION 0x01
#define GB_SVC_TYPE_INTF_DEVICE_ID 0x02
#define GB_SVC_TYPE_INTF_HOTPLUG 0x03
#define GB_SVC_TYPE_INTF_HOT_UNPLUG 0x04
#define GB_SVC_TYPE_INTF_RESET 0x05
#define GB_SVC_TYPE_CONN_CREATE 0x06
#define GB_SVC_TYPE_CONN_DESTROY 0x07

struct gb_svc_intf_device_id_request {
    uint8_t intf_id;
    uint8_t device_id;
};
/* device id response has no payload */

struct gb_svc_intf_hotplug_request {
    uint8_t intf_id;
    struct {
        uint32_t unipro_mfg_id;
        uint32_t unipro_prod_id;
        uint32_t ara_vend_id;
        uint32_t ara_prod_id;
    } data;
};
/* hotplug response has no payload */

struct gb_svc_intf_hot_unplug_request {
    uint8_t intf_id;
};
/* hot unplug response has no payload */

struct gb_svc_intf_reset_request {
    uint8_t intf_id;
};
/* interface reset response has no payload */

struct gb_svc_conn_create_request {
    uint8_t intf1_id;
    uint16_t cport1_id;
    uint8_t intf2_id;
    uint16_t cport2_id;
};
/* connection create response has no payload */

struct gb_svc_conn_destroy_request {
    uint8_t intf1_id;
    uint16_t cport1_id;
    uint8_t intf2_id;
    uint16_t cport2_id;
};
/* connection destroy response has no payload */

/* UART */

/* Version of the Greybus UART protocol we support */
#define GB_UART_VERSION_MAJOR 0x00
#define GB_UART_VERSION_MINOR 0x01

/* Greybus UART operation types */
#define GB_UART_TYPE_INVALID 0x00
#define GB_UART_TYPE_PROTOCOL_VERSION 0x01
#define GB_UART_TYPE_SEND_DATA 0x02
#define GB_UART_TYPE_RECEIVE_DATA 0x03 /* Unsolicited data */
#define GB_UART_TYPE_SET_LINE_CODING 0x04
#define GB_UART_TYPE_SET_CONTROL_LINE_STATE 0x05
#define GB_UART_TYPE_SET_BREAK 0x06
#define GB_UART_TYPE_SERIAL_STATE 0x07 /* Unsolicited data */

/* Represents data from AP -> Module */
struct gb_uart_send_data_request {
    uint16_t size;
    uint8_t data[];
};

/* Represents data from Module -> AP */
struct gb_uart_recv_data_request {
    uint16_t size;
    uint8_t data[];
};

struct gb_uart_set_line_coding_request {
    uint32_t rate;
    uint8_t format;
#define GB_SERIAL_1_STOP_BITS 0
#define GB_SERIAL_1_5_STOP_BITS 1
#define GB_SERIAL_2_STOP_BITS 2

    uint8_t parity;
#define GB_SERIAL_NO_PARITY 0
#define GB_SERIAL_ODD_PARITY 1
#define GB_SERIAL_EVEN_PARITY 2
#define GB_SERIAL_MARK_PARITY 3
#define GB_SERIAL_SPACE_PARITY 4

    uint8_t data_bits;
};

/* output control lines */
#define GB_UART_CTRL_DTR 0x01
#define GB_UART_CTRL_RTS 0x02

struct gb_uart_set_control_line_state_request {
    uint16_t control;
};

struct gb_uart_set_break_request {
    uint8_t state;
};

/* input control lines and line errors */
#define GB_UART_CTRL_DCD 0x01
#define GB_UART_CTRL_DSR 0x02
#define GB_UART_CTRL_BRK 0x04
#define GB_UART_CTRL_RI 0x08

#define GB_UART_CTRL_FRAMING 0x10
#define GB_UART_CTRL_PARITY 0x20
#define GB_UART_CTRL_OVERRUN 0x40

struct gb_uart_serial_state_request {
    uint16_t control;
};

struct op_msg {
    struct op_header header;
    union {
        struct protocol_version_rsp pv_rsp;
        struct gb_control_get_manifest_size_response control_msize_rsp;
        struct gb_control_get_manifest_response control_manifest_rsp;
        struct gb_gpio_line_count_response gpio_lc_rsp;
        struct gb_gpio_activate_request gpio_act_req;
        struct gb_gpio_deactivate_request gpio_deact_req;
        struct gb_gpio_get_direction_request gpio_get_dir_req;
        struct gb_gpio_get_direction_response gpio_get_dir_rsp;
        struct gb_gpio_direction_in_request gpio_dir_input_req;
        struct gb_gpio_direction_out_request gpio_dir_output_req;
        struct gb_gpio_get_value_request gpio_get_val_req;
        struct gb_gpio_get_value_response gpio_get_val_rsp;
        struct gb_gpio_set_value_request gpio_set_val_req;
        struct gb_gpio_set_debounce_request gpio_set_db_req;
        struct gb_gpio_irq_type_request gpio_irq_type_req;
        struct gb_gpio_irq_mask_request gpio_irq_mask_req;
        struct gb_gpio_irq_unmask_request gpio_irq_unmask_req;
        struct gb_gpio_irq_event_request gpio_irq_event_req;
        struct gb_i2c_functionality_response i2c_fcn_rsp;
        struct gb_i2c_transfer_request i2c_xfer_req;
        struct gb_i2c_transfer_response i2c_xfer_rsp;
        struct gb_pwm_count_response pwm_cnt_rsp;
        struct gb_pwm_activate_request pwm_act_req;
        struct gb_pwm_deactivate_request pwm_deact_req;
        struct gb_pwm_config_request pwm_cfg_req;
        struct gb_pwm_polarity_request pwm_pol_req;
        struct gb_pwm_enable_request pwm_enb_req;
        struct gb_pwm_disable_request pwm_dis_req;
        struct gb_i2s_mgmt_get_supported_configurations_response i2s_mgmt_get_sup_conf_rsp;
        struct gb_i2s_mgmt_get_processing_delay_response i2s_mgmt_get_proc_delay_rsp;
        struct gb_uart_send_data_request uart_send_data_req;
        struct gb_uart_recv_data_request uart_recv_data_rsp;
        struct gb_uart_set_break_request uart_sb_req;
        struct gb_uart_serial_state_request uart_ss_resp;
        struct gb_uart_set_line_coding_request uart_slc_req;
        struct gb_uart_set_control_line_state_request uart_sls_req;
    };
};

/* Matches up with the Greybus Protocol specification document */
#define GREYBUS_VERSION_MAJOR 0x00
#define GREYBUS_VERSION_MINOR 0x01

#define PROTOCOL_STATUS_SUCCESS 0x00
#define PROTOCOL_STATUS_INVALID 0x01
#define PROTOCOL_STATUS_NOMEM 0x02
#define PROTOCOL_STATUS_BUSY 0x03
#define PROTOCOL_STATUS_RETRY 0x04
#define PROTOCOL_STATUS_BAD 0xff

#define OP_RESPONSE 0x80

#ifdef _WIN32
#pragma warning(default : 4200)
#endif

#endif /* __GREYBUS_PROTOCOLS_H */
