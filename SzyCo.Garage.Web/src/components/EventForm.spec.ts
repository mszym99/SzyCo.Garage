import eventFormSource from "./EventForm.vue?raw";

describe("EventForm.vue", () => {
  it("clears event fields after saving", () => {
    expect(eventFormSource).toContain("function resetForm");
    expect(eventFormSource).toContain("carId: getDefaultCarId()");
    expect(eventFormSource).toContain("eventTypeId: null");
    expect(eventFormSource).toContain('PartName: ""');
    expect(eventFormSource).toContain('fallbackJsonData.value = "{}"');
    expect(eventFormSource).toContain("resetForm();");
  });

  it("supports editing existing events", () => {
    expect(eventFormSource).toContain("event?: EventModel | null");
    expect(eventFormSource).toContain("isEditing.value ? \"Edit Event\"");
    expect(eventFormSource).toContain("function hydrateForm()");
    expect(eventFormSource).toContain("new EventViewModel(props.event ?? undefined)");
  });

  it("does not expose raw JSON event data in the form", () => {
    expect(eventFormSource).not.toContain('label="Event Data"');
    expect(eventFormSource).not.toContain('v-model="rawJsonData"');
    expect(eventFormSource).toContain("fallbackJsonData");
    expect(eventFormSource).toContain("parseJsonData(fallbackJsonData.value)");
  });
});
