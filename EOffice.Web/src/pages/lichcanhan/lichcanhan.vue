<script>
import Swal from "sweetalert2";
import "@fullcalendar/core/vdom";

import FullCalendar from "@fullcalendar/vue3";

import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from "@fullcalendar/timegrid";
import interactionPlugin, {Draggable} from "@fullcalendar/interaction";
import bootstrapPlugin from "@fullcalendar/bootstrap";
import listPlugin from "@fullcalendar/list";

import {INITIAL_EVENTS, categories} from "./utils";

export default {
  components: {FullCalendar},
  data() {
    return {
      calendarOptions: {
        timeZone: "local",
        droppable: true,
        navLinks: true,
        plugins: [
          dayGridPlugin,
          timeGridPlugin,
          interactionPlugin,
          bootstrapPlugin,
          listPlugin,
        ],
        locale: "vi-VN",
        slotLabelFormat: {
          hour: "numeric",
          minute: "numeric",
          hour12: false,
        },
        themeSystem: "bootstrap",
        headerToolbar: {
          left: "prev,next today",
          center: "title",
          right: "dayGridMonth,timeGridWeek,timeGridDay,listMonth",
        },
        windowResize: () => {
          this.getInitialView();
        },
        initialView: this.getInitialView(),
        initialEvents: INITIAL_EVENTS,
        editable: true,
        selectable: true,
        selectMirror: true,
        dayMaxEvents: true,
        weekends: true,
        dateClick: this.dateClicked,
        eventClick: this.editEvent,
        eventsSet: this.handleEvents,
      },
      currentEvents: [],
      showModal: false,
      eventModal: false,
      categories: categories,
      submitted: false,
      submit: false,
      newEventData: {},
      edit: {},
      deleteId: {},
      event: {
        title: "",
        category: "",
      },
      editevent: {
        editTitle: "",
        editcategory: "",
      },
    };
  },
  methods: {
    getInitialView() {
      if (window.innerWidth >= 768 && window.innerWidth < 1200) {
        return "timeGridWeek";
      } else if (window.innerWidth <= 768) {
        return "listMonth";
      } else {
        return "dayGridMonth";
      }
    },
    dateClicked(info) {
      this.newEventData = info;
      this.showModal = true;
    },

    hideModal(e) {
      this.submitted = false;
      this.showModal = false;
      this.event = {};
    },

    editEvent(info) {
      this.edit = info.event;
      this.editevent.editTitle = this.edit.title;
      this.editevent.editcategory = this.edit.classNames[0];
      this.eventModal = true;
    },

    editSubmit(e) {
      this.submit = true;
      const editTitle = this.editevent.editTitle;
      const editcategory = this.editevent.editcategory;

      this.edit.setProp("title", editTitle);
      this.edit.setProp("classNames", editcategory);
      this.successmsg();
      this.eventModal = false;
    },
    closeModal() {
      this.eventModal = false;
    },

    handleEvents(events) {
      this.currentEvents = events;
    },

    handleSubmit(e) {
      this.submitted = true;

      // stop here if form is invalid
      this.v$.$touch();
      if (this.v$.$invalid) {
        return;
      } else {
        const title = this.event.title;
        const category = this.event.category;
        let calendarApi = this.newEventData.view.calendar;

        this.currentEvents = calendarApi.addEvent({
          id: this.newEventData.length + 1,
          title,
          start: this.newEventData.date,
          end: this.newEventData.date,
          classNames: [category],
        });
        this.successmsg();
        this.showModal = false;
        this.newEventData = {};
      }
      this.submitted = false;
      this.event = {};
    },

    confirm() {
      Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to delete this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#34c38f",
        cancelButtonColor: "#f46a6a",
        confirmButtonText: "Yes, delete it!",
      }).then((result) => {
        if (result.value) {
          this.deleteEvent();
          Swal.fire("Deleted!", "Event has been deleted.", "success");
        }
      });
    },
  },
};
</script>

<template>


</template>

<style></style>
