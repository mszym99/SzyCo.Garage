import eventFormSource from "./EventForm.vue?raw";

describe("EventForm.vue", () => {
  it("clears event fields after saving", () => {
    expect(eventFormSource).toContain("function resetForm");
    expect(eventFormSource).toContain("carId: getDefaultCarId()");
    expect(eventFormSource).toContain("eventTypeId: null");
    expect(eventFormSource).toContain("eventFieldValues.value = {}");
    expect(eventFormSource).toContain('fallbackJsonData.value = "{}"');
    expect(eventFormSource).toContain("resetForm();");
  });

  it("supports editing existing events", () => {
    expect(eventFormSource).toContain("event?: EventModel | null");
    expect(eventFormSource).toContain('isEditing.value ? "Edit Event"');
    expect(eventFormSource).toContain("function hydrateForm()");
    expect(eventFormSource).toContain(
      "new EventViewModel(props.event ?? undefined)",
    );
  });

  it("does not expose raw JSON event data in the form", () => {
    expect(eventFormSource).not.toContain('label="Event Data"');
    expect(eventFormSource).not.toContain('v-model="rawJsonData"');
    expect(eventFormSource).toContain("fallbackJsonData");
    expect(eventFormSource).toContain("parseJsonData(fallbackJsonData.value)");
  });

  it("renders event fields from JSON definitions", () => {
    expect(eventFormSource).toContain("selectedEventDefinitionFields");
    expect(eventFormSource).toContain("parseEventDefinition");
    expect(eventFormSource).toContain("normalizeEventFieldDefinition");
    expect(eventFormSource).toContain('v-model="eventFieldValues[field.name]"');
  });

  it("filters archived cars from the add-event car picker", () => {
    expect(eventFormSource).toContain("!car.isArchived");
    expect(eventFormSource).toContain("car.carId === eventForm.value.carId");
  });
});
