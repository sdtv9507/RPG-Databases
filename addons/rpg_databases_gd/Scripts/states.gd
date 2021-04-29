extends Container
tool

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var iconPath = ""
var state_selected = 0
var add_string = -1
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func start():
	var json_dictionary = get_parent().get_parent().call("read_data", "State")
	$StateButton.clear()
	for i in range(json_dictionary.size()):
		var state_data = json_dictionary["state" + String(i)]
		if i > $StateButton.get_item_count() - 1:
			$StateButton.add_item(state_data["name"])
		else:
			$StateButton.set_item_text(i, state_data["name"])
	refresh_data(state_selected)

func refresh_data(id):
	var json_dictionary = get_parent().get_parent().call("read_data", "State")
	var state_data = json_dictionary["state" + String(id)]
	var erase_conditions = state_data["erase_conditions"]
	var messages = state_data["messages"]
	var custom_erase_conditions = state_data["custom_erase_conditions"]
	$NameLabel/NameLine.text = state_data["name"]
	var icon = state_data["icon"]
	if icon != "":
		iconPath = String(state_data["icon"])
		$IconLabel/Sprite.texture = load(state_data["icon"])
	$RestrictionLabel/RestrictionOption.selected = int(state_data["restriction"])
	$PriorityLabel/PriorityValue.value = int(state_data["priority"])
	$EraseLabel/TurnsLabel/MinTurns.value = int(erase_conditions["turns_min"])
	$EraseLabel/TurnsLabel/MaxTurns.value = int(erase_conditions["turns_max"])
	$EraseLabel/DamageLabel/Damage.value = int(erase_conditions["erase_damage"])
	$EraseLabel/SetpsLabel/SpinBox.value = int(erase_conditions["erase_setps"])
	$MessagesLabel/PanelContainer/VBoxContainer/MessageList.clear()
	for message in messages.keys():
		$MessagesLabel/PanelContainer/VBoxContainer/MessageList.add_item(message)
	$CustomEraseLabel/PanelContainer/VBoxContainer/EraseConditions.clear()
	for condition in custom_erase_conditions.keys():
		$CustomEraseLabel/PanelContainer/VBoxContainer/EraseConditions.add_item(condition)
	if (state_data.has("effects") == true):
		clear_effect_list()
		var effect_list = state_data["effects"]
		for effect in effect_list:
			add_effect_list(effect["name"], int(effect["data_id"]), String(effect["value1"]), String(effect["value2"]))

func _on_SearchState_pressed():
	$IconLabel/SearchDialog.popup_centered()

func _on_SearchStateIconDialog_file_selected(path):
	iconPath = path
	$IconLabel/Sprite.texture = load(path)

func _on_AddCustomStateCondition_pressed():
	add_string = 0
	$AddString.window_title = "Custom State Erase Formula"
	$AddString.popup_centered()

func _on_RemoveCustomStateCondition_pressed():
	var selected = $CustomEraseLabel/PanelContainer/VBoxContainer/EraseConditions.GetselectedItems()[0]
	if (selected > -1):
		$CustomEraseLabel/PanelContainer/VBoxContainer/EraseConditions.remove_item(selected)

func _on_AddStateMessage_pressed():
	add_string = 1
	$AddString.window_title = "State Message"
	$AddString.popup_centered()

func _on_RemoveStateMessage_pressed():
	var selected = $MessagesLabel/PanelContainer/VBoxContainer/MessageList.GetselectedItems()[0]
	if (selected > -1):
		$MessagesLabel/PanelContainer/VBoxContainer/MessageList.remove_item(selected)

func _on_ConfirmAddString_pressed():
	var text = $AddString/LineEdit.text
	if (add_string == 0):
		$CustomEraseLabel/PanelContainer/VBoxContainer/EraseConditions.add_item(text)
	elif (add_string == 1):
		$MessagesLabel/PanelContainer/VBoxContainer/MessageList.add_item(text)
	add_string = -1
	$AddString.hide()

func _on_CancelAddString_pressed():
	add_string = -1
	$AddString.hide()

func _on_StateButton_item_selected(id):
	state_selected = id
	refresh_data(id)

func _on_AddState_pressed():
	$StateButton.add_item("NewState")
	var id = $StateButton.get_item_count() - 1
	var json_dictionary = get_parent().get_parent().call("read_data", "State")
	var state_data:Dictionary
	var erase_condition:Dictionary
	var messages:Dictionary
	var custom_erase_conditions:Dictionary
	state_data["name"] = "NewState"
	state_data["icon"] = ""
	state_data["restriction"] = 4
	state_data["priority"] = 100
	erase_condition["turns_min"] = 0
	erase_condition["turns_max"] = 0
	erase_condition["erase_damage"] = 0
	erase_condition["erase_setps"] = 0
	state_data["erase_conditions"] = erase_condition
	messages["0"] = "Insert a custom message"
	state_data["messages"] = messages
	custom_erase_conditions["0"] = "Insert a custom condition"
	state_data["custom_erase_conditions"] = custom_erase_conditions
	json_dictionary["state" + String(id)] = state_data
	get_parent().get_parent().call("store_data", "State", json_dictionary)

func _on_RemoveState_pressed():
	var json_dictionary = get_parent().get_parent().call("read_data", "State")
	if (json_dictionary.keys().size() > 1):
		var state_id = state_selected
		while (state_id < json_dictionary.keys().size() - 1):
			json_dictionary["state" + String(state_id)] = json_dictionary["state" + String(state_id + 1)]
			state_id += 1
		json_dictionary.erase("state" + String(state_id))
		get_parent().get_parent().call("store_data", "State", json_dictionary)
		$StateButton.remove_item(state_selected)
		if (state_selected == 0):
			$StateButton.select(state_selected + 1)
			state_selected += 1
		else:
			$StateButton.select(state_selected - 1)
			state_selected -= 1
		$StateButton.select(state_selected)
		refresh_data(state_selected)

func _on_SaveStates_pressed():
	var json_dictionary = get_parent().get_parent().call("read_data", "State")
	var state_data = json_dictionary["state" + String(state_selected)]
	var erase_condition = state_data["erase_conditions"]
	var messages = state_data["messages"]
	var custom_erase_conditions = state_data["custom_erase_conditions"]
	var effect_list: Array

	state_data["name"] = $NameLabel/NameLine.text
	state_data["icon"] = iconPath
	state_data["restriction"] = $RestrictionLabel/RestrictionOption.selected
	state_data["priority"] = $PriorityLabel/PriorityValue.value
	erase_condition["turns_min"] = $EraseLabel/TurnsLabel/MinTurns.value
	erase_condition["turns_max"] = $EraseLabel/TurnsLabel/MaxTurns.value
	erase_condition["erase_damage"] = $EraseLabel/DamageLabel/Damage.value
	erase_condition["erase_setps"] = $EraseLabel/SetpsLabel/SpinBox.value
	var items = $MessagesLabel/PanelContainer/VBoxContainer/MessageList.get_item_count()
	for i in range(items):
		messages[String(i)] = $MessagesLabel/PanelContainer/VBoxContainer/MessageList.get_item_text(i)
	items = $CustomEraseLabel/PanelContainer/VBoxContainer/EraseConditions.get_item_count()
	for i in range(items):
		custom_erase_conditions[String(i)] = $CustomEraseLabel/PanelContainer/VBoxContainer/EraseConditions.get_item_text(i)
	var effect_size = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_item_count()
	for i in range(effect_size):
		var effect_data: Dictionary
		effect_data["name"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_item_text(i)
		effect_data["data_id"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.get_item_text(i)
		effect_data["value1"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.get_item_text(i)
		effect_data["value2"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.get_item_text(i)
		effect_list.append(effect_data)
	state_data["effects"] = effect_list
	get_parent().get_parent().call("store_data", "State", json_dictionary)
	refresh_data(state_selected)

func _on_AddStateEffect_pressed() -> void:
	get_parent().get_parent().call("open_effect_manager", "States")

func _on_RemoveStateEffect_pressed() -> void:
	var id = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_selected_items()[0]
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.remove_item(id)

func add_effect_list(name: String, data_id: int, value1: String, value2: String) -> void:
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.add_item(name)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.add_item(String(data_id))
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.add_item(value1)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.add_item(value2)

func clear_effect_list() -> void:
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.clear()
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.clear()
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.clear()
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.clear()

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
